using ConsoleApp.Drivers.I2C;

namespace Application.Devices.Fan
{
    internal class Fan : IFan
    {
        private uint address;
        II2C driver;

        public Fan(uint address, II2C driver)
        {
            this.address = address;
            this.driver = driver;
        }

        public void DecreaseSpeed()
        {
            driver.send(address, "0x0B");
        }

        public void IncreasSpeed()
        {
            driver.send(address, "0x0A");
        }

    }
}
