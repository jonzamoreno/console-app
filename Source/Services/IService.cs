namespace ConsoleApp.Services;

public enum ServiceState
{
    Ok, 
    Error, 
    Stop,
}


public interface IService
{
    void Start();
    void Stop();
    ServiceState GetState();
}