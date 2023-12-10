using Installer.Packages.Docker.Interface;
using Installer.Program;

namespace Installer.Packages.Docker.Model
{
    public class Docker : ProgramObj, IDocker
    {
        private DockerController controller;

        public Docker()
        {
            controller = new DockerController();
        }

        public DockerController Controller { get => controller; }

        public void Initialize()
        {
            try
            {
                controller.StartDocker();
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
