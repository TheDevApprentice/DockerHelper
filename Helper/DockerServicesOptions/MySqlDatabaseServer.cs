using Installer.ComputerSystem.Model;
using Installer.ComputerSystem.ProcessObj;
using Installer.Packages.Docker;
using Installer.Program;
using Installer.Program.UserInputs;

namespace Installer.DockerServicesOptions
{
    internal class MySqlDatabaseServer : ServerOption
    {
        public override string Name => "MySql Database Creator";

        public override string Description => "An helper to create an mySql database docker container";

        public override string Type => "Database";

        private string _output;

        public override string Output => _output;

        private DockerCompose _dockerCompose;

        #region Configure

        public override bool Configure()
        {
            Console.Clear();

            ProgramInfo.DisplayInstallerTitle();
            Console.WriteLine("");

            ProgramInfo.DisplayOptionInfirmation(this.Name, this.Description, this.Type);
            ProgramInfo.DisplayConfigurationInProcess();

            ExecuteConfigure(out bool configurationEnded);

            if (!configurationEnded)
            {
                return false;
            }

            return configurationEnded;
        }

        public override void ExecuteConfigure(out bool configurationEnded)
        {
            bool configureTable = false;
            string containerName = UserInput.GetUserInput("Enter container name:");
            Console.WriteLine("");
            string username = UserInput.GetUserInput("Enter SQL Server username:");
            Console.WriteLine("");
            string password = UserInput.GetUserInput("Enter SQL Server password:");
            Console.WriteLine("");
            string dbName = UserInput.GetUserInput("Enter SQL Server database name:");
            Console.WriteLine("");

            if (UserInput.AskYesNoQuestion("Do you want to create tables for this database ?"))
            {
                ConfigureTables();
                configureTable = true;
            }

            _dockerCompose = new DockerCompose();
            _dockerCompose.GenerateDockerComposeMySql(containerName, username, password, dbName, configureTable);

            configurationEnded = true;
        }

        private bool ConfigureTables()
        {
            bool sucess = false;
            try
            {
                List<Table> tables = new List<Table>();
                string sqlScript = "";

                while (UserInput.AskYesNoQuestion("Create table ?"))
                {
                    ProgramInfo.DisplaySqlTablesCreatedAutmaticaalyInformation();

                    string tableName = UserInput.GetUserInput("Enter table name:");
                    tables.Add(new Table { TableName = tableName });
                }

                foreach (var table in tables)
                {
                    sqlScript += $"-- Create table {table.TableName}\n";
                    sqlScript += $"CREATE TABLE {table.TableName} (\n";
                    sqlScript += $"    ID INT PRIMARY KEY,\n";
                    sqlScript += $"    Name VARCHAR(50)\n";
                    sqlScript += $");\n\n";
                }

                string fullPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "init-script.sql");
                File.WriteAllText(fullPath, sqlScript);

                Console.WriteLine($"SQL script generated and saved to {fullPath}");
                sucess = true;
            }
            catch (Exception)
            {
                Console.WriteLine("An error occured while configuring tables");
            }

            return sucess;
        }

        #endregion

        #region Install 

        public override bool Install()
        {
            Console.Clear();

            ProgramInfo.DisplayInstallerTitle();
            Console.WriteLine("");

            Console.WriteLine("L'installation est en cours...");

            ExecuteInstall(out bool executionEnded);

            Console.WriteLine("\nInstallation terminée !");

            Console.WriteLine();
            Console.WriteLine($"The docker compose has been created at {_dockerCompose.DockerPath}");
            Console.WriteLine();
            Console.WriteLine($"Docker compose configuration : ");
            Console.WriteLine($"{_dockerCompose.dockercomposeFileRepresentation}");

            ProgramInfo.DisplayExitInformation();

            if (!executionEnded)
            {
                return false;
            }

            return executionEnded;
        }

        public override void ExecuteInstall(out bool installationEnded)
        {
            string executionDirectory = _output;

            ProcessRunner processRunner = new ProcessRunner("docker-compose", "up", true);
            processRunner.Run();

            installationEnded = true;
        }

        #endregion
    }

    internal class Table
    {
        public string TableName { get; set; }
    }
}
