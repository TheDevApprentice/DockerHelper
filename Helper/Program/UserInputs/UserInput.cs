namespace Installer.Program.UserInputs
{
    public static class UserInput
    {
        public static bool AskYesNoQuestion(string question)
        {
            Console.WriteLine($"{question} (yes/no): ");
            string response = Console.ReadLine().ToLower();
            return response == "yes" || response == "y";
        }

        public static string GetUserInput(string prompt)
        {
            Console.WriteLine(prompt);
            return Console.ReadLine();
        }
    }
}
