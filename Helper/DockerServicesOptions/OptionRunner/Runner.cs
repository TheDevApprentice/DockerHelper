using Installer.ComputerSystem.Model;

namespace Installer.DockerServicesOptions.OptionRunner
{
    public class Runner
    {
        private RunnerController _runnerController;
        private ServerOption _option;

        public RunnerController RunnerController { get => _runnerController; }
        public ServerOption Option { get => _option; }

        public Runner(ServerOption option)
        {
            _option = option;
            _runnerController = new RunnerController();
        }

        public void Run()
        {
            try
            {
                _runnerController.RunConfigurator(out bool configurationEnded, _option);

                try
                {
                    if (configurationEnded)
                    {
                        _runnerController.RunInstalator(out bool executionEnded, _option);
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
