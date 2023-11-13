using ConsoleApp.Services;
using ConsoleApp.Logging;
using ConsoleApp.Drivers.I2C;
using Moq;
using Application.Devices.TemperatureSensor;
using Application.Devices.Fan;

namespace FanTests;


public class FanControlTests
{
    [Test]
    public void Create()
    {
        var loggerMock = new Mock<ILogger>();
        var temp = new Mock<ITemperatureSensor>();
        var fan = new Mock<IFan>();
        FanControlSvc sut = new FanControlSvc(1000,loggerMock.Object, temp.Object, fan.Object);
        Assert.That(sut.GetState(), Is.EqualTo(ServiceState.Ok));
    }

    [Test]
    public void FanDecreasedInTemperatureBelowLimit()
    {
        var loggerMock = new Mock<ILogger>();
        var temp = new Mock<ITemperatureSensor>();
        var fan = new Mock<IFan>();
        temp.Setup(t => t.GetTemperature()).Returns(25.0).Verifiable();
        fan.Setup(f => f.DecreaseSpeed()).Verifiable();
        FanControlSvc sut = new FanControlSvc(500, loggerMock.Object, temp.Object, fan.Object); Assert.Pass();
        sut.Start();
        temp.Verify();
        fan.Verify();
    }
}