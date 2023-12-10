using Installer.ComputerSystem.Model;
using Installer.ComputerSystem.ProcessObj;
using Installer.Packages.Docker;
using Installer.Program;

namespace Installer.DockerServicesOptions
{
    internal class JuypiterNotebookServerInNginx : ServerOption
    {
        public override string Name => "Rocker Chat Server Creator";

        public override string Description => "An helper to create a Rocker Chat Server docker container";

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
            //bool configurePersistence = UserInput.AskYesNoQuestion("Do you want to enable persistence for Redis?");

            DockerCompose dockerCompose = new DockerCompose();
            _output = dockerCompose.GenerateDockerComposeJupyerInNginx();

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
            string executionDirectory = Output;

            ProcessRunner processRunner = new ProcessRunner("docker-compose", "up", true, executionDirectory);
            processRunner.Run();

            installationEnded = true;
        }

        #endregion
    }
}
