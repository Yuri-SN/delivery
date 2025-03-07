using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class TransportShould
{
    public static IEnumerable<object[]> GetTransports()
    {
        yield return [TransportType.Pedestrian, 1, "pedestrian", 1];
        yield return [TransportType.Bicycle, 2, "bicycle", 2];
        yield return [TransportType.Car, 3, "car", 3];
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void ReturnCorrectIdAndName(TransportType transportType, int id, string name, int speed)
    {
        // Arrange
        // Act
        // Assert
        transportType.Id.Should().Be(id);
        transportType.Name.Should().Be(name);
        transportType.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundById(TransportType transportType, int id, string name, int speed)
    {
        // Arrange
        // Act
        var result = TransportType.FromId(id).Value;

        // Assert
        result.Should().Be(transportType);
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Speed.Should().Be(speed);
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void CanBeFoundByName(TransportType transportType, int id, string name, int speed)
    {
        // Arrange
        // Act
        var result = TransportType.FromName(name).Value;

        // Assert
        result.Should().Be(transportType);
        result.Id.Should().Be(id);
        result.Name.Should().Be(name);
        result.Speed.Should().Be(speed);
    }

    [Fact]
    public void ReturnErrorWhenTransportNotFoundById()
    {
        // Arrange
        var id = -1;

        // Act
        var result = TransportType.FromId(id);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnErrorWhenTransportNotFoundByName()
    {
        // Arrange
        var name = "not-existed-transportType";

        // Act
        var result = TransportType.FromName(name);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Fact]
    public void ReturnListOfStatuses()
    {
        // Arrange
        // Act
        var allStatuses = TransportType.List();

        // Assert
        allStatuses.Should().NotBeEmpty();
    }

    [Fact]
    public void DerivedEntity()
    {
        // Arrange
        // Act
        var isDerivedEntity = typeof(TransportType).IsSubclassOf(typeof(Entity<int>));

        // Assert
        isDerivedEntity.Should().BeTrue();
    }

    [Fact]
    public void BeEqualWhenIdIsEqual()
    {
        // Arrange
        var pedestrian1 = TransportType.Pedestrian;
        var pedestrian2 = TransportType.Pedestrian;
        pedestrian1.Id.Should().Be(pedestrian2.Id);

        // Act
        var result = pedestrian1.Equals(pedestrian2);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public void NotBeEqualWhenIdIsNotEqual()
    {
        // Arrange
        var pedestrian = TransportType.Pedestrian;
        var car = TransportType.Car;
        pedestrian.Id.Should().NotBe(car.Id);

        // Act
        var result = pedestrian.Equals(car);

        // Assert
        result.Should().BeFalse();
    }
}