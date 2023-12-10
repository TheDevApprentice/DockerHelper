using System.Diagnostics;
using System.ServiceProcess;

namespace Installer.Packages.Docker
{
    public class DockerController
    {
        private DockerInstaller installer;
        private DockerUpdater DockerUpdater;
        private DockerNetwork DockerNetwork;
        private DockerVolume DockerVolume;
        private DockerContainer DockerContainer;

        public DockerInstaller Installer { get => installer; }
        public DockerUpdater DockerUpdater1 { get => DockerUpdater; }
        public DockerNetwork DockerNetwork1 { get => DockerNetwork; }
        public DockerVolume DockerVolume1 { get => DockerVolume; }
        public DockerContainer DockerContainer1 { get => DockerContainer; }

        public DockerController()
        {
            installer = new DockerInstaller();
            DockerUpdater = new DockerUpdater();
        }

        public void StartDocker()
        {
            Console.WriteLine("Starting docker.");
            StartDockerDesktop();
            Console.WriteLine("Done");
            Console.WriteLine("");
        }

        private void StartDockerDesktop()
        {
            if (IsDockerServiceRunning())
            {
                Console.WriteLine("Error: Docker service is already running. Please stop the service before starting Docker Desktop.");
                return;
            }

            string dockerDesktopPath = @"C:\Program Files\Docker\Docker\Docker Desktop.exe";

            if (File.Exists(dockerDesktopPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = dockerDesktopPath,
                    Arguments = "-WindowStyle Minimized",
                    UseShellExecute = false,
                };

                try
                {
                    Process.Start(startInfo);
                    //Console.WriteLine("Docker Desktop started successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Error: Docker Desktop not found at the specified path.");
            }
        }

        private bool IsDockerServiceRunning()
        {
            ServiceController sc = new ServiceController("com.docker.service");

            try
            {
                return sc.Status == ServiceControllerStatus.Running;
            }
            catch (InvalidOperationException)
            {
                // Le service n'existe pas
                return false;
            }
        }

        public void CreateDockerNetwork()
        {
            DockerNetwork = new DockerNetwork();
        }

        public void CreateDockerVolume()
        {
            DockerVolume = new DockerVolume();
        }

        public void CreateDockerContainer()
        {
            DockerContainer = new DockerContainer(DockerNetwork, DockerVolume);
        }
    }
}
