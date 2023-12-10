using System.Diagnostics;

namespace Installer.ComputerSystem.ProcessObj
{
    internal class ProcessRunner : Process
    {
        private string _fileName;
        private string _arguments;
        private string _executionDirectory;

        private Thread _monThread;
        private Process _process;
        private bool _isDockerComposeAction;

        public string Output;
        public int ExitCode;
        public Process Process { get => _process; }
        bool isFinished = false;

        public ProcessRunner(string fileName, string arguments = null, bool isDockerComposeAction = false, string executionDirectory = "")
        {
            _fileName = fileName;
            _arguments = arguments ?? string.Empty;
            _isDockerComposeAction = isDockerComposeAction;
            _executionDirectory = executionDirectory;
        }

        public void Run()
        {
            _monThread = new Thread(RunProces);
            _monThread.Name = _fileName.ToString() + " " + _arguments.ToString();

            //Console.WriteLine($"Demarrage du thread {_fileName.ToString()}.");

            _monThread.Start();
        }

        private void RunProces()
        {
            using (_process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = _fileName,
                    Arguments = _arguments,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    WorkingDirectory = _executionDirectory,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    ErrorDialog = true
                },
                EnableRaisingEvents = true,

            })
            {
                if (_isDockerComposeAction)
                {
                    _process.Exited += (sender, e) =>
                    {
                        Console.WriteLine("La commande Docker Compose s'est terminée.");
                    };

                    _process.OutputDataReceived += (sender, e) =>
                    {
                        if (e.Data != null)
                        {
                            Console.WriteLine(e.Data);

                            //if (e.Data.ToLower().Contains("ready for connections."))
                            //{
                            //    Console.WriteLine("Le conteneur est prêt pour la connexion.");
                            //    isFinished = true;
                            //    _process.CancelOutputRead();
                            //}
                            //else if (e.Data.Contains("ready for connections."))
                            //{

                            //}
                        }
                    };

                    _process.ErrorDataReceived += (sender, e) => Console.WriteLine(e.Data);

                    Console.WriteLine();
                    Console.WriteLine($"Démarrage du container docker ");
                }

                _process.Start();

                if (_isDockerComposeAction)
                {
                    Console.WriteLine();
                    Console.WriteLine($"Lecture des logs");
                    Console.WriteLine($"Analyse : ");

                    _process.BeginOutputReadLine();
                    _process.BeginErrorReadLine();
                }

                _process.WaitForExit();

                if (_process.HasExited)
                {
                    ExitCode = _process.ExitCode;

                    if (ExitCode != 0)
                    {
                        Console.WriteLine($"Failed to execute command. Exiting program.");
                        throw new Exception("Command execution failed.");
                    }

                    //Console.WriteLine($"Execute success. Exiting process.");
                    //Console.WriteLine($"Le process {process.Name} a terminé.");

                    _process.Kill();
                    //Console.WriteLine($"Le thread {_fileName.ToString() + " " + _arguments.ToString()} a terminé.");
                    _monThread.Join();
                }
                else
                {
                    _monThread.Join();
                }
            }
        }
    }
}