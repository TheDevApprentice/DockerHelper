namespace Installer.Packages.Chocolatey.Interface
{
    public interface IChocolatey
    {
        void Initialize();
        void Restart();
        void Shutdown();
    }
}