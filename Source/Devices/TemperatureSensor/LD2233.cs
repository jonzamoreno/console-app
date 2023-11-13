using ConsoleApp.Drivers.I2C;


namespace Application.Devices.TemperatureSensor
{
    internal class LD2233 : ITemperatureSensor
    {
        private readonly uint i2cAddress;
        private readonly II2C driver;

        public LD2233(II2C i2cDriver, uint addr)
        {
            this.driver = i2cDriver;
            this.i2cAddress = addr;
        }

        public double GetTemperature()
        {
            driver.send(i2cAddress, "0xFFFA");
            return double.Parse(driver.read(i2cAddress));
        }
    }
}
