using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;
using Application.Devices.TemperatureSensor;
using Application.Devices.Fan;

namespace ConsoleApp.Services;

public class FanControlSvc : IService
{
    private readonly ILogger logger;
    private readonly ITemperatureSensor sensor;
    private readonly IFan fan;
    private readonly uint refreshTime;

    ServiceState state;

    public FanControlSvc(uint refreshMillisecs, ILogger logger, ITemperatureSensor temperatureSensor, IFan fanControl)
    {
        this.sensor = temperatureSensor;
        this.fan = fanControl;
        this.logger = logger;
        this.refreshTime = refreshMillisecs;
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
                    double meas =this.sensor.GetTemperature();
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
                        this.fan.DecreaseSpeed();
                    else if (average > TargetTemperature)
                        this.fan.IncreasSpeed();

                    Thread.Sleep((int)refreshTime);
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