namespace ConsoleApp.Logging;

public interface ILogger
{
    void Info(string message);
    void Error(string message);
    void Debug(string message); 
}