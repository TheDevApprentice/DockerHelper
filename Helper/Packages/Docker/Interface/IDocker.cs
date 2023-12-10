namespace Installer.Packages.Docker.Interface
{
    public interface IDocker
    {
        DockerController Controller { get; }

        void Initialize();
        void Restart();
        void Shutdown();
    }
}