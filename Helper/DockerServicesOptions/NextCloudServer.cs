using Installer.ComputerSystem.Model;
using Installer.ComputerSystem.ProcessObj;
using Installer.Packages.Docker;
using Installer.Program;
using Installer.Program.UserInputs;

namespace Installer.DockerServicesOptions
{
    internal class NextCloudServer : ServerOption
    {
        public override string Name => "Next CLoud Server Creator";

        public override string Description => "An helper to create a NextCloud server using Docker";

        public override string Type => "Share file servers";

        private string _output;

        public override string Output => _output;

        private DockerCompose _dockerCompose;

        #region Configure

        public override bool Configure()
        {
            Console.Clear();

            ProgramInfo.DisplayInstallerTitle();
            Console.WriteLine();

            ProgramInfo.DisplayOptionInfirmation(this.Name, this.Description, this.Type);
            ProgramInfo.DisplayConfigurationInProcess();

            ExecuteConfigure(out bool configurationEnded);

            if (!configurationEnded)
            {
                return false;
            }

            return configurationEnded;
        }

        public override void ExecuteConfigure(out bool configurationEnded)
        {
            string containerName = UserInput.GetUserInput("Enter container name:");
            Console.WriteLine();

            string dbContainerName = UserInput.GetUserInput("Enter database container name:");
            Console.WriteLine();

            string dbUsername = UserInput.GetUserInput("Enter database user name:");
            Console.WriteLine();

            string dbPassword = UserInput.GetUserInput($"Enter database password for user : {dbUsername}");
            Console.WriteLine();

            string dbRootPassword = UserInput.GetUserInput("Enter database administrator password for root:");
            Console.WriteLine();

            _dockerCompose = new DockerCompose();
            _output = _dockerCompose.GenerateDockerComposeNextcloud(dbUsername, containerName, dbContainerName, dbPassword, dbRootPassword);

            configurationEnded = true;
        }

        #endregion

        #region Install 

        public override bool Install()
        {
            Console.Clear();

            ProgramInfo.DisplayInstallerTitle();
            Console.WriteLine();

            Console.WriteLine("Installation is in progress...");

            ExecuteInstall(out bool executionEnded);

            if (!executionEnded)
            {
                Console.WriteLine(".An error occured during the installation process");
                return false;
            }

            Console.WriteLine("\nInstallation completed!");

            Console.WriteLine();
            Console.WriteLine($"The docker compose has been created at {_dockerCompose.DockerPath}");
            Console.WriteLine();
            Console.WriteLine($"Docker compose configuration : ");
            Console.WriteLine($"{_dockerCompose.dockercomposeFileRepresentation}");

            ProgramInfo.DisplayExitInformation();
            return executionEnded;
        }

        public override void ExecuteInstall(out bool installationEnded)
        {
            string executionDirectory = _output;

            ProcessRunner processRunner = new ProcessRunner("docker-compose", "up", true, executionDirectory);
            processRunner.Run();

            installationEnded = true;
        }

        #endregion
    }
}
