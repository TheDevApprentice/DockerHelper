using Installer.Packages.Chocolatey.Interface;
using Installer.Program;

namespace Installer.Packages.Chocolatey
{
    public class Chocolatey : ProgramObj, IChocolatey
    {
        private ChocolateyController chocolateyController;

        public Chocolatey()
        {
            chocolateyController = new ChocolateyController();
        }

        public void Initialize()
        {
            try
            {
                if (!chocolateyController.IsChocolateyInstalled())
                {
                    Console.WriteLine("Chocolatey is not installed. We need to install it to manage the installation of different parts of the program.");
                    Console.WriteLine("Do you want to install Chocolatey? [Y/N]");

                    string installChocolatey = Console.ReadLine();

                    if (!string.IsNullOrEmpty(installChocolatey) && installChocolatey.ToUpper() == "Y")
                    {
                        chocolateyController.Installer.InstallChocolatey();
                    }
                    else
                    {
                        Console.WriteLine("Chocolatey installation aborted. Exiting program.");
                        throw new Exception("Chocolatey not installed.");
                    }
                }

                chocolateyController.ChocolateyUpdater.UpdateChocolatey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public override void Install()
        {
            base.Install();
        }

        public override void Restart()
        {
            throw new NotImplementedException();
        }

        public override void Shutdown()
        {
            throw new NotImplementedException();
        }
    }
}
