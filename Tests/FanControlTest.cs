using ConsoleApp.Services;
using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;
using Moq;

namespace FanTests;


public class FanControlTests
{

    [Test]
    public void Test1()
    {
        var loggerMock = new Mock<ILogger>();
        var i2cMock = new Mock<II2C>();

        FanControlSvc sut = new FanControlSvc(loggerMock.Object, i2cMock.Object);
        Assert.Pass();
    }
}