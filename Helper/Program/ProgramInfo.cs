namespace Installer.Program
{
    public class ProgramInfo : IDisposable
    {
        public static ProgramInfo Instance { get; private set; }

        static ProgramInfo()
        {
            Instance = new ProgramInfo();
        }

        public static void DisplayInstallerTitle()
        {
            Console.WriteLine(@"
                  ______     ______     __         ______   __         ______     __     __         ______    
                 /\  ___\   /\  __ \   /\ \       /\  == \ /\ \       /\  ___\   /\ \   /\ \       /\  == \   
                 \ \ \____  \ \ \/\ \  \ \ \____  \ \  _-/ \ \ \____  \ \ \____  \ \ \  \ \ \____  \ \  __<   
                  \ \_____\  \ \_____\  \ \_____\  \ \_\    \ \_____\  \ \_____\  \ \_\  \ \_____\  \ \_\ \_\
                   \/_____/   \/_____/   \/_____/   \/_/     \/_____/   \/_____/   \/_/   \/_____/   \/_/ /_/
            ");
        }

        public static void DisplayWelcome()
        {
            Console.WriteLine();

            Console.WriteLine("Welcome !");
            Console.WriteLine();

            Console.WriteLine("This tool is made to help you create server with docker");
        }

        public static void DisplayPreviewQuestion()
        {
            Console.WriteLine();
            Console.WriteLine("You can choose within multiple options to create different servers and configurations: ");
            Console.WriteLine();
        }

        public static void DisplayExitInformation()
        {
            Console.WriteLine();
            Console.WriteLine("Informations : ");
            Console.WriteLine();
            Console.WriteLine("     - Press any key to get back to the options menu.");
            Console.WriteLine("     - Press esc key to quit the program.");

            Console.ReadKey();
        }

        public static void DisplayOptionInfirmation(string name, string description, string type)
        {
            Console.WriteLine($"Welcome in the {name}");
            Console.WriteLine("");
            Console.WriteLine($"Description: {description}\n");
            Console.WriteLine($"Type: {type} \n");
        }

        public static void DisplayConfigurationInProcess()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Configuration is in progress...");
            Console.WriteLine("");
        }

        public static void DisplayInstallationnInProcess()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine("Installation is in progress...");
            Console.WriteLine("");
        }

        public static void DisplaySqlTablesCreatedAutmaticaalyInformation()
        {
            Console.WriteLine("----------------------------------------");
            Console.WriteLine($"ID and Name will be created automatically");
            Console.WriteLine($"");
        }

        void IDisposable.Dispose()
        {
            DisposeInstance();
        }

        public static void DisposeInstance()
        {
            if (Instance != null)
            {
                Instance.Dispose();
                Instance = null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Dispose managed resources
                // Add any additional cleanup code for managed objects here

                // For example, if you have disposable objects, call their Dispose method
                // disposableObject.Dispose();
            }

            // Dispose unmanaged resources
            // Add any additional cleanup code for unmanaged objects here

            // For example, if you have unmanaged resources, release them here
            // ReleaseUnmanagedResources();
        }

        ~ProgramInfo()
        {
            Dispose(false);
        }
    }
}
