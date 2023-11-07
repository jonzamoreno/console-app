using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;

namespace ConsoleApp.Services;

public class FanControlSvc : IService
{
    private ILogger logger;
    private II2C i2cDriver;
    ServiceState state;

    public FanControlSvc()
    {
        this.i2cDriver = new I2CDummy();
        this.logger = new ConsoleLogger();
    }

    public void Start()
    {
        Task.Run(() =>
        {
            try
            {
                const double TargetTemperature = 38.0;
                double[] measurements = new double[10];
                int lastMeasureIndex = 0;
                state = ServiceState.Ok;
                while (state == ServiceState.Ok)
                {
                    // Read current measurement from sensor.
                    i2cDriver.send(0x80, "0xFFFA");
                    double meas = double.Parse(i2cDriver.read(0x80));
                    logger.Info($"Measured {meas}. Saved on {lastMeasureIndex}");

                    // Save in historical values
                    measurements[lastMeasureIndex] = meas;
                    lastMeasureIndex++;
                    if (lastMeasureIndex >= 10) lastMeasureIndex = 0;

                    // Calculate average
                    var average = measurements.Average();
                    logger.Info($"Computed average: {average}");

                    // Control loop
                    if (average < TargetTemperature)
                        // Decrease FAN speed
                        i2cDriver.send(0x81, "0x0B");
                    else if (average > TargetTemperature)
                        //Increase FAN speed
                        i2cDriver.send(0x81, "0xA");

                    Thread.Sleep(5000);
                }
                state = ServiceState.Stop;
            }
            catch (IOException e)
            {
                logger.Error($"Communication error : {e}");
                state = ServiceState.Error;
            }
            catch (Exception e)
            {
                logger.Error($"UNEXPECTED ERROR : {e}");
                state = ServiceState.Error;
            }
        });
    }

    public void Stop()
    {
        state = ServiceState.Stop;
    }

    public ServiceState GetState()
    {
        return this.state;
    }

}