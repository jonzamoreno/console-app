
namespace ConsoleApp.Drivers.I2C;

public interface II2C
{
    void send(uint address, string command);
    string read(uint address);

}