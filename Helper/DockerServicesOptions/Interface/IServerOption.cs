namespace Installer.DockerServicesOptions.Interface
{
    internal interface IServerOption
    {
        bool Configure();
        bool Install();
    }
}