using DeliveryApp.Core.Domain.Model.CourierAggregate;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

using FluentAssertions;
using Xunit;

public class TransportShould
{
    [Fact]
    public void ReturnCorrectIdAndName()
    {
        // Arrange
        // Act
        var pedestrian = Transport.Pedestrian;
        var bicycle = Transport.Bicycle;
        var car = Transport.Car;

        // Assert
        pedestrian.Id.Should().Be(1);
        pedestrian.Name.Should().Be("pedestrian");
        pedestrian.Speed.Should().Be(1);

        bicycle.Id.Should().Be(2);
        bicycle.Name.Should().Be("bicycle");
        bicycle.Speed.Should().Be(2);

        car.Id.Should().Be(3);
        car.Name.Should().Be("car");
        car.Speed.Should().Be(3);
    }
    
    [Theory]
    [InlineData(1, "pedestrian")]
    [InlineData(2, "bicycle")]
    [InlineData(3, "car")]
    public void CanBeFoundById(int id, string name)
    {
        // Arrange
        // Act
        var transport =  Transport.FromId(id).Value;

        // Assert
        transport.Id.Should().Be(id);
        transport.Name.Should().Be(name);
    }

    [Fact]
    public void ReturnErrorWhenStatusNotFoundById()
    {
        // Arrange
        var id = -1;

        // Act
        var result = Transport.FromId(id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
    
    [Theory]
    [InlineData(1, "pedestrian")]
    [InlineData(2, "bicycle")]
    [InlineData(3, "car")]
    public void CanBeFoundByName(int id, string name)
    {
        // Arrange
        // Act
        var transport = Transport.FromName(name).Value;

        // Assert
        transport.Id.Should().Be(id);
        transport.Name.Should().Be(name);
    }
    
    [Fact]
    public void ReturnErrorWhenStatusNotFoundByName()
    {
        // Arrange
        var name = "not-existed-status";

        // Act
        var result = Transport.FromName(name);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }
    
    [Fact]
    public void ReturnListOfTransports()
    {
        // Arrange
        // Act
        var allTransports = Transport.List();

        // Assert
        allTransports.Should().NotBeEmpty();
    }
}
