using Installer.ComputerSystem.Model;
using Installer.ComputerSystem.ProcessObj;
using Installer.Packages.Docker;
using Installer.Program;
using Installer.Program.UserInputs;

namespace Installer.DockerServicesOptions
{
    internal class RedisServer : ServerOption
    {
        public override string Name => "Redis Server Creator";

        public override string Description => "An helper to create a Redis server docker container";

        public override string Type => "Server";

        private string _output;

        public override string Output => _output;

        #region Configure

        public override bool Configure()
        {
            Console.Clear();

            ProgramInfo.DisplayInstallerTitle();
            Console.WriteLine("");

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
            bool configurePersistence = UserInput.AskYesNoQuestion("Do you want to enable persistence for Redis?");
            DockerCompose dockerCompose = new DockerCompose();

            _output = dockerCompose.GenerateDockerComposeRedis(configurePersistence);

            configurationEnded = true;
        }

        #endregion

        #region Install 

        public override bool Install()
        {
            Console.Clear();
            ProgramInfo.DisplayInstallerTitle();

            ProgramInfo.DisplayConfigurationInProcess();

            ExecuteInstall(out bool installationEnded);

            if (installationEnded)
            {
                Console.WriteLine("\nInstallation completed!");
            }
            else
            {
                Console.WriteLine("\nAn error occured");
            }

            ProgramInfo.DisplayExitInformation();
            return installationEnded;
        }

        public override void ExecuteInstall(out bool installationEnded)
        {
            string executionDirectory = _output;

            ProcessRunner processRunner = new ProcessRunner("docker-compose", "up", true);
            processRunner.Run();

            installationEnded = true;
        }

        #endregion
    }
}
