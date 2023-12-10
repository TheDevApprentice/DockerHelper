using Installer.ComputerSystem.ProcessObj;

namespace Installer.Packages.Chocolatey
{
    public class ChocolateyInstaller
    {
        public void InstallChocolatey()
        {
            ProcessRunner processRunner = new ProcessRunner("powershell.exe", "-NoProfile -ExecutionPolicy Bypass -Command \"iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))\"");
            processRunner.Run();

            if (processRunner.ExitCode == 0)
            {
                Console.WriteLine("Chocolatey installed successfully.");
            }
            else
            {
                Console.WriteLine("Failed to install Chocolatey. Exiting program.");
                throw new Exception("Chocolatey installation failed.");
            }
        }
    }
}
