using Installer.ComputerSystem.InformationsSystem;
using Installer.ComputerSystem.Model;
using Installer.DockerServicesOptions;
using Installer.DockerServicesOptions.OptionRunner;
using Installer.Packages.Chocolatey;
using Installer.Packages.Docker.Model;
using Installer.Program;

class Program
{
    static void Main()
    {
        ProgramInfo.DisplayInstallerTitle();
        ProgramInfo.DisplayWelcome();

        try
        {
            Startup_Initialization();
            AskOption();

            Environment.Exit(0);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"{ex.Message}");
            Environment.Exit(-1);
        }
    }

    #region Startup

    private static void Startup_Initialization()
    {
        try
        {
            InformationsSystem informationsSystem = new InformationsSystem();
            informationsSystem.GetSystemInformationAsync();

            Chocolatey chocolatey = new Chocolatey();
            chocolatey.Initialize();

            Docker docker = new Docker();
            docker.Initialize();
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            throw new Exception(ex.Message);
        }
    }

    #endregion

    #region Option

    private static void AskOption()
    {
        bool escapeKeyPressed = false;

        try
        {
            string[] options = { "Sql Database", "Redis Server", "GitLab Server", "NextCloud Server", "Jupyter Notebook Server", "Jupyter Notebook Ngxinx Server" };
            int selectedIndex = 0;

            while (!escapeKeyPressed)
            {
                Console.Clear();
                ProgramInfo.DisplayInstallerTitle();
                ProgramInfo.DisplayWelcome();
                ProgramInfo.DisplayPreviewQuestion();

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.Gray;
                        Console.ForegroundColor = ConsoleColor.Black;
                    }

                    Console.WriteLine($"{i + 1}. {options[i]}");

                    Console.ResetColor();
                }

                ConsoleKeyInfo key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = Math.Max(0, selectedIndex - 1);
                        break;

                    case ConsoleKey.DownArrow:
                        selectedIndex = Math.Min(options.Length - 1, selectedIndex + 1);
                        break;

                    case ConsoleKey.Enter:
                        int backup = selectedIndex;
                        int index = backup + 1;

                        if (index == 1)
                        {
                            ExecuteOption(1);
                        }
                        else if (index == 2)
                        {
                            ExecuteOption(2);
                        }
                        else if (index == 3)
                        {
                            ExecuteOption(3);
                        }
                        else if (index == 4)
                        {
                            ExecuteOption(4);
                        }
                        else if (index == 5)
                        {
                            ExecuteOption(5);
                        }
                        else if (index == 6)
                        {
                            ExecuteOption(6);
                        }
                        break;
                }
            }

            ProgramInfo.DisposeInstance();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    private static void ExecuteOption(int optionNumber)
    {
        Console.Clear();
        ProgramInfo.DisplayInstallerTitle();
        Console.WriteLine("");

        ServerOption option = null;

        if (optionNumber == 1)
            option = new MySqlDatabaseServer();
        if (optionNumber == 2)
            option = new RedisServer();
        if (optionNumber == 3)
            option = new GitLabServer();
        if (optionNumber == 4)
            option = new NextCloudServer();
        if (optionNumber == 5)
            option = new JuypiterNotebookServer();
        if (optionNumber == 6)
            option = new JuypiterNotebookServerInNginx();

        Runner runner = new Runner(option);
        runner.Run();
    }

    #endregion
}