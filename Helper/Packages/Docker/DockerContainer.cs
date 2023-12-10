namespace Installer.Packages.Docker
{
    public class DockerContainer
    {
        private DockerNetwork _dockerNetwork;
        private DockerVolume _dockerVolume;

        public DockerNetwork DockerNetwork { get => _dockerNetwork; }
        public DockerVolume DockerVolume { get => _dockerVolume; }

        private string _name;
        private string _version;
        private string[] _ports;
        private string uudi;

        public string Name { get => _name; }
        public string Version { get => _version; }
        public string[] Ports { get => _ports; }
        public string Uudi { get => uudi; }

        public DockerContainer(DockerNetwork dockerNetwork, DockerVolume dockerVolume)
        {
            _dockerNetwork = dockerNetwork;
            _dockerVolume = dockerVolume;
        }

        public void GetDockerContainer()
        {

        }

        public void Start(string uuid)
        {

        }

        public void Shutdown(string uuid)
        {

        }

        public void Restart(string uuid)
        {

        }
    }
}
