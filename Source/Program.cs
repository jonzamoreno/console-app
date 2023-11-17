using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;
using ConsoleApp.Services;
using Application.Devices.TemperatureSensor;
using Application.Devices.Fan;

namespace ConsoleApp;

class Program 
{
    static void Main(string[] args)
    {

        // Boostrap
        //@todo: Substitute this code for a container
        ILogger logger = new ConsoleLogger();
        II2C i2cDriver = new I2CDummy();
        List<IService> services = new List<IService>();
        ITemperatureSensor sensor = new LD2233(i2cDriver, 0x80);
        IFan fan = new Fan(0x81, i2cDriver);
        services.Add(new FanControlSvc(5000, logger, sensor, fan));

        // Run
        try
        {
            foreach(var svc in services)
                svc.Start();

            logger.Info("Press Q to quit..");
            while (Console.ReadKey().Key != ConsoleKey.Q) ;

            // End
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
