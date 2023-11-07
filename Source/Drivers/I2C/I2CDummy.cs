namespace ConsoleApp.Drivers.I2C;


public class I2CDummy : II2C
{
    public void send(uint address, string command)
    {

    }
    public string read(uint address)
    {
        Random rnd = new Random(); 
        const double min = 15.0;
        const double max = 80.0;
        var v = min + (rnd.NextDouble() * (max-min));
        return v.ToString();
    }
}