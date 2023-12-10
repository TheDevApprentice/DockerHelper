using Installer.DockerServicesOptions.Interface;

namespace Installer.ComputerSystem.Model
{
    public abstract class ServerOption : IServerOption
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract string Type { get; }

        public abstract string Output { get ; }


        public abstract bool Configure();
        public abstract void ExecuteConfigure(out bool configurationEnded);

        public abstract bool Install();
        public abstract void ExecuteInstall(out bool configurationEnded);
    }
}
