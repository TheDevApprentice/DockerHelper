namespace Installer.Program
{
    public abstract class ProgramObj
    {
        public abstract void Shutdown();

        public abstract void Restart();

        public virtual void Install()
        {

        }
    }
}
