using Installer.ComputerSystem.ProcessObj;

namespace Installer.Packages.Chocolatey
{
    public class ChocolateyUpdater
    {
        public void UpdateChocolatey()
        {
            Console.WriteLine("Choco update chocolatey.");

            ProcessRunner processRunner = new ProcessRunner("powershell.exe", "-NoProfile -ExecutionPolicy Bypass -Command \"iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))\"");
            processRunner.Run();
            //runner.RunnerController.RunCommand("choco upgrade chocolatey -y");
            Console.WriteLine("Done");
            Console.WriteLine("");
        }
    }
}
