using System.Reflection.Metadata;
using System.Runtime;

namespace ConsoleApp;

class Program 
{
    static void Main(string[] args)
    {
        try
        {
            const double TargetTemperature = 38.0;
            double[] measurements = new double[10];
            int lastMeasureIndex = 0;
            while(true)
            {
                // Read current measurement from sensor.
                i2cSend(0x80, "0xFFFA");
                double meas = double.Parse(i2cRead(0x80));
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"INFO: Measured {meas}. Saved on {lastMeasureIndex}");

                // Save in historical values
                measurements[lastMeasureIndex] = meas;
                lastMeasureIndex++;
                if(lastMeasureIndex >= 10)lastMeasureIndex=0;

                // Calculate average
                var average = measurements.Average();
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Info: Computed average: {average}");

                // Control loop
                if(average < TargetTemperature)
                    // Decrease FAN speed
                    i2cSend(0x81, "0x0B");
                else if (average > TargetTemperature)
                    //Increase FAN speed
                    i2cSend(0x81, "0xA");

                Thread.Sleep(5000);
            }
        }
        catch(IOException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Communication error : {e}");
            Environment.Exit(1);
        }
        catch (Exception e)
        { 
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"UNEXPECTED ERROR : {e}");
            Environment.Exit(1);
        }

        Environment.Exit(0);
    }

    static void i2cSend(uint address, string command)
    {

    }

    static string i2cRead(uint address)
    {
        Random rnd = new Random(); 
        const double min = 15.0;
        const double max = 80.0;
        var v = min + (rnd.NextDouble() * (max-min));
        return v.ToString();
    }
}
