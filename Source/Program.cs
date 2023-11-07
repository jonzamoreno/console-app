using System.Reflection.Metadata;
using System.Runtime;
using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;
using ConsoleApp.Services;

namespace ConsoleApp;

class Program 
{
    static void Main(string[] args)
    {
        ILogger logger = new ConsoleLogger();

        try
        {
            List<IService> services = new List<IService>();
            services.Add(new FanControlSvc());
            foreach(var svc in services)
                svc.Start();

            logger.Info("Press Q to quit..");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;


            foreach (var svc in services)
                svc.Stop();
        }
        catch (IOException e)
        {
            logger.Error($"Communication error : {e}");
            Environment.Exit(-1);
        }
        catch (Exception e)
        { 
            logger.Error($"UNEXPECTED ERROR : {e}");
            Environment.Exit(-1);
        }

        Environment.Exit(0);
    }
}
