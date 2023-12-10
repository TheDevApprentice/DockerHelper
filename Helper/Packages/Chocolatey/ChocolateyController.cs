namespace Installer.Packages.Chocolatey
{
    public class ChocolateyController
    {
        private ChocolateyInstaller installer;
        private ChocolateyUpdater chocolateyUpdater;

        public ChocolateyInstaller Installer { get => installer; }
        public ChocolateyUpdater ChocolateyUpdater { get => chocolateyUpdater; }

        public ChocolateyController()
        {
            installer = new ChocolateyInstaller();
            chocolateyUpdater = new ChocolateyUpdater();
        }

        public bool IsChocolateyInstalled()
        {
            //ProcessRunner processRunner = new ProcessRunner("choco upgrade", "");
            //processRunner.Run();

            //string output = processRunner.Output;
            //return output.Contains("Chocolatey");
            return true;
        }
    }
}
