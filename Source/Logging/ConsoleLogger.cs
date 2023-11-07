using System.Runtime;

namespace ConsoleApp;

public class ConsoleLogger : ILogger
{
    public void Info(string message)
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        this.log($"INFO - {message}");
    }

    public void Error(string message)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        this.log($"ERROR - {message}");
    }

    public void Debug(string message)
    {
        Console.ForegroundColor = ConsoleColor.White;
        this.log($"DEBUG - {message}");
    }

    private void log(string message)
    {
        Console.WriteLine($"{DateTime.Now} - {message}");
    }
}
