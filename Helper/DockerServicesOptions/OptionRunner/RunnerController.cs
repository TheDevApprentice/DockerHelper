using Installer.ComputerSystem.Model;

namespace Installer.DockerServicesOptions.OptionRunner
{
    public class RunnerController
    {
        public bool RunConfigurator(out bool configurationEnded, ServerOption option)
        {
            configurationEnded = false;
            Console.Clear();
            Console.WriteLine("Configuration est en cours...");

            if (option.Configure())
            {
                configurationEnded = true;
                Console.WriteLine("\nConfiguration terminée !");
            }

            Console.WriteLine("\nConfiguration did not succeed !");
            return configurationEnded;
        }

        public bool RunInstalator(out bool executionEnded, ServerOption option)
        {
            executionEnded = false;
            Console.Clear();
            Console.WriteLine("L'installation est en cours...");

            option.Install();

            //Console.WriteLine("\nInstallation terminée !");
            //Console.ReadLine();
            executionEnded = true;
            return executionEnded;
        }
    }
}
