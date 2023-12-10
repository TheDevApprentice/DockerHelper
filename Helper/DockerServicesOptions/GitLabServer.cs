using Installer.ComputerSystem.Model;
using Installer.ComputerSystem.ProcessObj;
using Installer.Packages.Docker;
using Installer.Program;
using Installer.Program.UserInputs;

namespace Installer.DockerServicesOptions
{
    internal class GitLabServer : ServerOption
    {
        public override string Name => "GitLab Server Creator";

        public override string Description => "An helper to create a GitLab server using Docker";

        public override string Type => "Version Control";

        private DockerCompose _dockerCompose;

        private string _output;

        public override string Output => _output;

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

            string gitLabPassword = UserInput.GetUserInput("Enter GitLab administrator password:");
            Console.WriteLine();

            _dockerCompose = new DockerCompose();
            _dockerCompose.GenerateDockerComposeGitLab(containerName, gitLabPassword);

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
            ProcessRunner processRunner = new ProcessRunner("docker-compose", "up", true);
            processRunner.Run();

            installationEnded = true;
        }

        #endregion
    }
}
