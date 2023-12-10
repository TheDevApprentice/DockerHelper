using Installer.ComputerSystem.ProcessObj;

namespace Installer.ComputerSystem.InformationsSystem
{
    public class InformationsSystem
    {
        public async Task<List<object>> GetSystemInformationAsync()
        {
            var tasks = new List<Task<List<object>>>
            {
                GetInformationAsync("Win32_OperatingSystem", "Caption, Version, BuildNumber, Manufacturer, OSArchitecture, LastBootUpTime, InstallDate, ServicePackMajorVersion, ServicePackMinorVersion, RegisteredUser, SerialNumber"),
                GetInformationAsync("Win32_Processor", "Name, Manufacturer, MaxClockSpeed, NumberOfCores, NumberOfLogicalProcessors, Architecture, Description"),
                GetInformationAsync("Win32_ComputerSystem", "Manufacturer, Model, TotalPhysicalMemory, SystemType, Domain, Workgroup, PartOfDomain, UserName"),
                GetInformationAsync("Win32_DiskDrive", "DeviceID, Model, Size, MediaType, InterfaceType"),
            };

            var results = await Task.WhenAll(tasks);
            return results.SelectMany(result => result).ToList();
        }

        private async Task<List<object>> GetInformationAsync(string className, string properties, string filter = null)
        {
            var command = $"Get-CimInstance -ClassName {className}" +
                          (filter != null ? $" | Where-Object {{{filter}}} " : "") +
                          $" | Select-Object {properties}";

            return await RunCommandInformationAsync(command);
        }

        private async Task<List<object>> RunCommandInformationAsync(string command)
        {
            var informationObjects = new List<object>();

            var processRunner = new ProcessRunner("powershell.exe", $"-NoProfile -ExecutionPolicy Bypass -Command \"{command}\"");
            processRunner.Run();

            informationObjects.AddRange(ParseOutput(processRunner.Output));

            return informationObjects;
        }

        private List<object> ParseOutput(string output)
        {
            var informationObjects = new List<object>();

            var sections = output.Split(new[] { Environment.NewLine + Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            Parallel.ForEach(sections, section =>
            {
                var lines = section.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

                if (lines.Length > 0)
                {
                    var firstLine = lines[0].Trim();
                    switch (firstLine)
                    {
                        case "Name        FullName    Description                    Disabled PasswordRequired":
                            informationObjects.Add(ParseUserInformation(lines));
                            break;
                        case "Caption    Version    BuildNumber    Manufacturer             OSArchitecture LastBootUpTime             InstallDate               ServicePackMajorVersion ServicePackMinorVersion RegisteredUser             SerialNumber":
                            informationObjects.Add(ParseOperatingSystemInformation(lines));
                            break;
                        case "Name                  Manufacturer             MaxClockSpeed NumberOfCores NumberOfLogicalProcessors Architecture Description":
                            informationObjects.Add(ParseProcessorInformation(lines));
                            break;
                        case "Manufacturer Model                  TotalPhysicalMemory SystemType      Domain Workgroup PartOfDomain UserName":
                            informationObjects.Add(ParseComputerSystemInformation(lines));
                            break;
                        case "Manufacturer           Version                 ReleaseDate              SerialNumber":
                            informationObjects.Add(ParseBiosInformation(lines));
                            break;
                        case "DeviceID      Model                               Size     MediaType                  InterfaceType":
                            informationObjects.Add(ParseDiskDriveInformation(lines));
                            break;
                        case "Name                                      AdapterType                             MACAddress          Speed                NetConnectionStatus":
                            informationObjects.Add(ParseNetworkAdapterInformation(lines));
                            break;
                        case "Name ScreenWidth ScreenHeight BitsPerPixel RefreshRate":
                            informationObjects.Add(ParseDisplayInformation(lines));
                            break;
                        case "Name                     Version                  Vendor":
                            informationObjects.Add(ParseInstalledSoftwareInformation(lines));
                            break;
                        default:
                            //Console.WriteLine($"Unknown information type: {firstLine}");
                            break;
                    }
                }
            });

            return informationObjects;
        }

        private UserInformation ParseUserInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new UserInformation
            {
                Name = values[0],
                FullName = values[1],
                Description = values[2],
                Disabled = bool.Parse(values[3]),
                PasswordRequired = bool.Parse(values[4])
            };
        }

        private OperatingSystemInformation ParseOperatingSystemInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new OperatingSystemInformation
            {
                Caption = values[0],
                Version = values[1],
                BuildNumber = values[2],
                Manufacturer = values[3],
                OSArchitecture = values[4],
                LastBootUpTime = values[5],
                InstallDate = values[6],
                ServicePackMajorVersion = values[7],
                ServicePackMinorVersion = values[8],
                RegisteredUser = values[9],
                SerialNumber = values[10]
            };
        }

        private ProcessorInformation ParseProcessorInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new ProcessorInformation
            {
                Name = values[0],
                Manufacturer = values[1],
                MaxClockSpeed = values[2],
                NumberOfCores = int.Parse(values[3]),
                NumberOfLogicalProcessors = int.Parse(values[4]),
                Architecture = values[5],
                Description = values[6]
            };
        }

        private ComputerSystemInformation ParseComputerSystemInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new ComputerSystemInformation
            {
                Manufacturer = values[0],
                Model = values[1],
                TotalPhysicalMemory = values[2],
                SystemType = values[3],
                Domain = values[4],
                Workgroup = values[5],
                PartOfDomain = bool.Parse(values[6]),
                UserName = values[7]
            };
        }

        private BiosInformation ParseBiosInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new BiosInformation
            {
                Manufacturer = values[0],
                Version = values[1],
                ReleaseDate = values[2],
                SerialNumber = values[3]
            };
        }

        private DiskDriveInformation ParseDiskDriveInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new DiskDriveInformation
            {
                DeviceID = values[0],
                Model = values[1],
                Size = values[2],
                MediaType = values[3],
                InterfaceType = values[4]
            };
        }

        private NetworkAdapterInformation ParseNetworkAdapterInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new NetworkAdapterInformation
            {
                Name = values[0],
                AdapterType = values[1],
                MACAddress = values[2],
                Speed = values[3],
                NetConnectionStatus = values[4]
            };
        }

        private DisplayInformation ParseDisplayInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new DisplayInformation
            {
                Name = values[0],
                ScreenWidth = values[1],
                ScreenHeight = values[2],
                BitsPerPixel = values[3],
                RefreshRate = values[4]
            };
        }

        private InstalledSoftwareInformation ParseInstalledSoftwareInformation(string[] lines)
        {
            var values = lines[1].Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return new InstalledSoftwareInformation
            {
                Name = values[0],
                Version = values[1],
                Vendor = values[2]
            };
        }
    }


    #region Objects 

    public class InformationsSystems
    {
        public override string ToString()
        {
            return base.ToString();
        }
    }

    public class UserInformation : InformationsSystems
    {
        public string Name { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public bool Disabled { get; set; }
        public bool PasswordRequired { get; set; }
    }

    public class OperatingSystemInformation : InformationsSystems
    {
        public string Caption { get; set; }
        public string Version { get; set; }
        public string BuildNumber { get; set; }
        public string Manufacturer { get; set; }
        public string OSArchitecture { get; set; }
        public string LastBootUpTime { get; set; }
        public string InstallDate { get; set; }
        public string ServicePackMajorVersion { get; set; }
        public string ServicePackMinorVersion { get; set; }
        public string RegisteredUser { get; set; }
        public string SerialNumber { get; set; }
    }

    public class ProcessorInformation : InformationsSystems
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string MaxClockSpeed { get; set; }
        public int NumberOfCores { get; set; }
        public int NumberOfLogicalProcessors { get; set; }
        public string Architecture { get; set; }
        public string Description { get; set; }
    }

    public class ComputerSystemInformation : InformationsSystems
    {
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string TotalPhysicalMemory { get; set; }
        public string SystemType { get; set; }
        public string Domain { get; set; }
        public string Workgroup { get; set; }
        public bool PartOfDomain { get; set; }
        public string UserName { get; set; }
    }

    public class BiosInformation : InformationsSystems
    {
        public string Manufacturer { get; set; }
        public string Version { get; set; }
        public string ReleaseDate { get; set; }
        public string SerialNumber { get; set; }
    }

    public class DiskDriveInformation : InformationsSystems
    {
        public string DeviceID { get; set; }
        public string Model { get; set; }
        public string Size { get; set; }
        public string MediaType { get; set; }
        public string InterfaceType { get; set; }
    }

    public class NetworkAdapterInformation : InformationsSystems
    {
        public string Name { get; set; }
        public string AdapterType { get; set; }
        public string MACAddress { get; set; }
        public string Speed { get; set; }
        public string NetConnectionStatus { get; set; }
    }

    public class DisplayInformation : InformationsSystems
    {
        public string Name { get; set; }
        public string ScreenWidth { get; set; }
        public string ScreenHeight { get; set; }
        public string BitsPerPixel { get; set; }
        public string RefreshRate { get; set; }
    }

    public class InstalledSoftwareInformation : InformationsSystems
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public string Vendor { get; set; }
    }

    public class DiskSpaceInfo
    {
        public long TotalSize { get; set; }
        public long FreeSpace { get; set; }
    }

    #endregion
}
