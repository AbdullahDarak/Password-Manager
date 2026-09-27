using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
namespace C_Fundamentals.ConsoleApp.TrainingApps.PasswordManager;

public  class HistoryService
{

    public HistoryService()
    {
        if (!File.Exists("History.txt"))
        {
            AnyStep("info", "History File Initilized");
        }
    }
    public void AnyStep(string type  , string content)
    { 
        string Time = DateTime.Now.ToString();
        string formatType = $"[{type.ToUpper()}]".PadRight(12);
        File.AppendAllText("History.txt", $"{Time} | {formatType} | {content}{Environment.NewLine}");

    }
}

