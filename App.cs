using System.Text;
namespace C_Fundamentals.ConsoleApp.TrainingApps.PasswordManager;


// كشف الباسوردات المكررة

internal class App
{

    private static Dictionary<string, string> _passwordsEntries = new();
    private static string _passwordOfMyApp = "0000";
    private static HistoryService historyService = new HistoryService();

    public static void Main(string[] args)
    {
        int cnt = 0;
        const int attempts = 3;

        while (true)
        {

            Console.Write("Enter The Password of your Application : ");
            var input = Console.ReadLine();
            Console.WriteLine();

            if (input.Equals(_passwordOfMyApp))
            {
                historyService.AnyStep("INFO", "Successeful Login.");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Successful Login! Welcome to your Password Manager.");
                Console.ResetColor();
                Console.WriteLine();
                break;
            }
            else
            {
                cnt++;
                if (cnt >= attempts)
                {
                    historyService.AnyStep("CRITICAL", "Application Locked Down due to too many Attempts.");
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Too Many Failed Attempts! Application is Shutting for Security reasons.");
                    Console.ResetColor();
                    Environment.Exit(0);
                }
                else
                {
                    historyService.AnyStep("WARN", $"Invalid Password. Attempt {cnt}:{attempts}");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("Invalid Password. Try Again.");
                    Console.WriteLine($"Remember that you only have {attempts - cnt} attempts.");
                    Console.ResetColor();
                    Console.WriteLine();

                }
            }
        }
        ReadPasswords();
        while (true)
        {
            Console.WriteLine("Select An Option : ");
            Console.WriteLine("1. List all Passwords.");
            Console.WriteLine("2. Add/Change  Password.");
            Console.WriteLine("3. Delete  Password.");
            Console.WriteLine("4. Get Password.");

            Console.WriteLine();
            var option = Console.ReadLine();

            Console.WriteLine();
            if (option == "1")
            {
                ListAllPasswords();
            }
            else if (option == "2")
            {
                AddOrChangePassword();
            }
            else if (option == "3")
            {

                DeletePassword();
            }
            else if (option == "4")
            {
                GetPassword();
            }
            else
            {
                Console.WriteLine("Invalid Option.");
            }
            Console.WriteLine("_____________________________________");
            Console.WriteLine();
        }
    }
    private static void GetPassword()
    {

        Console.Write("Enter WebSiteName : ");
        var webSiteName = Console.ReadLine();
        Console.WriteLine();
        if (_passwordsEntries.ContainsKey(webSiteName))
        {
            historyService.AnyStep("WARN", $" User requested to view password for: {webSiteName}");
            Console.WriteLine($"Your Password={_passwordsEntries[webSiteName]}");
        }

        else
        {
            Console.WriteLine($"No Password Exist for {webSiteName}");
        }
    }
    private static void DeletePassword()
    {

        Console.Write("Enter WebSiteName to Delete its Password : ");

        var webSiteName = Console.ReadLine();
        Console.WriteLine();
        if (_passwordsEntries.ContainsKey(webSiteName))
        {
            historyService.AnyStep("INFO", $"The {webSiteName} Password has been Deleted");
            _passwordsEntries.Remove(webSiteName);
        }
        else
        {
            Console.WriteLine($"No Password Exist for {webSiteName}");
        }

        SavePasswords();

    }
    private static void AddOrChangePassword()
    {
        Console.Write("Enter WebSiteName : ");
        var webSiteName = Console.ReadLine();
        webSiteName = webSiteName.ToLower();
        Console.WriteLine();
        Console.Write("Enter Your Password : ");
        var password = Console.ReadLine();

        if (_passwordsEntries.ContainsKey(webSiteName))  // edit
        {
            historyService.AnyStep("INFO", $"The {webSiteName} Password has been changed");
            _passwordsEntries[webSiteName] = password;
        }

        else    // add 
        {


            while (_passwordsEntries.ContainsValue(password))
            {

                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("This Password alreday Exist to another  Website !");
                Console.Write("Enter a different password (recomended), or press (cse) to keep it anyway: ");
                Console.ResetColor();

                var newpassword = Console.ReadLine();

                if (string.IsNullOrEmpty(newpassword) || newpassword.Equals("cse", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }
                password = newpassword;
            }

            historyService.AnyStep("INFO", $"Added new Password for {webSiteName}");
            _passwordsEntries.Add(webSiteName, password);
        }

        SavePasswords();
    }
    private static void ListAllPasswords()
    {
        historyService.AnyStep("WARN", $"User requested  to view All Passwords *_*");
        foreach (var entry in _passwordsEntries)
        {
            Console.WriteLine(entry.Key + "=" + entry.Value);
        }
    }
    private static void ReadPasswords()
    {
        if (File.Exists("passwords.txt"))
        {
            var passwordLines = File.ReadAllText("passwords.txt");
            foreach (var line in passwordLines.Split(Environment.NewLine))
            {
                if (!(string.IsNullOrEmpty(line)))
                {

                    int indxOfEqual = line.IndexOf("=");
                    string webSiteName = line.Substring(0, indxOfEqual);
                    string password = line.Substring(indxOfEqual + 1);
                    _passwordsEntries.Add(webSiteName, Encryption.Decrypt(password));
                }
            }
        }
    }
    private static void SavePasswords()
    {
        var sb = new StringBuilder();
        foreach (var entry in _passwordsEntries)
        {
            sb.AppendLine($"{entry.Key}={Encryption.Encrypt(entry.Value)}");
        }
        File.WriteAllText("passwords.txt", sb.ToString());

    }
}
