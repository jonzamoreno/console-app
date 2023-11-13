using Application.Devices.TemperatureSensor;
using ConsoleApp.Drivers.I2C;
using Moq;
using System.Net;

namespace FanTests;

public class LD2233Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TemperatureParsing()
    {
        var i2c = new Mock<II2C>();
        i2c.Setup(i => i.send(0x81, "0xFFFA")).Verifiable();
        i2c.Setup(i => i.read(0x81)).Returns("20.0").Verifiable();
        ITemperatureSensor s = new LD2233(i2c.Object, 0x81);
        Assert.That(s.GetTemperature(), Is.EqualTo(20.0));
        i2c.Verify();
    }

    [Test]
    public void TemperatureParsingError()
    {
        var i2c = new Mock<II2C>();
        i2c.Setup(i => i.read(0x81)).Returns("WRONGDATA").Verifiable();
        ITemperatureSensor s = new LD2233(i2c.Object, 0x81);
        Assert.Throws<FormatException>(() => s.GetTemperature());
    }
}