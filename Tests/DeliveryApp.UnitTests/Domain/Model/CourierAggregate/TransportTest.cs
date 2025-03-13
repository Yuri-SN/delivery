using System;
using System.Collections.Generic;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using FluentAssertions;
using Xunit;
using DeliveryApp.Core.Domain.Model.SharedKernel;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class TransportShould
{
    public static IEnumerable<object[]> GetTransports()
    {
        yield return ["Pedestrian", 1];
        yield return ["Bicycle", 2];
        yield return ["Car", 3];
    }
    
    public static IEnumerable<object[]> GetIncorrectTransportParams()
    {
        yield return ["", 2];
        yield return ["Bicycle", -1];
    }
    
    public static IEnumerable<object[]> GetTransportsAndLocations()
    {
        // Пешеход, заказ X:совпадает, Y: совпадает
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value
        ];

        // Пешеход, заказ X:совпадает, Y: выше
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(1, 1).Value, Location.Create(1, 5).Value, Location.Create(1, 2).Value
        ];

        // Пешеход, заказ X:правее, Y: совпадает
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(2, 2).Value, Location.Create(3, 2).Value, Location.Create(3, 2).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(5, 5).Value, Location.Create(6, 5).Value, Location.Create(6, 5).Value
        ];

        // Пешеход, заказ X:правее, Y: выше
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(2, 2).Value, Location.Create(3, 3).Value, Location.Create(3, 2).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(1, 1).Value, Location.Create(5, 5).Value, Location.Create(2, 1).Value
        ];

        // Пешеход, заказ X:совпадает, Y: ниже
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(1, 2).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(5, 5).Value, Location.Create(5, 1).Value, Location.Create(5, 4).Value
        ];

        // Пешеход, заказ X:левее, Y: совпадает
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(2, 2).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(5, 5).Value, Location.Create(1, 5).Value, Location.Create(4, 5).Value
        ];

        // Пешеход, заказ X:левее, Y: ниже
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(2, 2).Value, Location.Create(1, 1).Value, Location.Create(1, 2).Value
        ];
        yield return
        [
            Transport.Create("Pedestrian",1).Value, Location.Create(5, 5).Value, Location.Create(1, 1).Value, Location.Create(4, 5).Value
        ];


        // Велосипедист, заказ X:совпадает, Y: совпадает
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value, Location.Create(5, 5).Value];

        // Велосипедист, заказ X:совпадает, Y: выше
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(1, 3).Value, Location.Create(1, 3).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(1, 5).Value, Location.Create(1, 3).Value];

        // Велосипедист, заказ X:правее, Y: совпадает
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(2, 2).Value, Location.Create(4, 2).Value, Location.Create(4, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(8, 5).Value, Location.Create(7, 5).Value];

        // Велосипедист, заказ X:правее, Y: выше
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(2, 2).Value, Location.Create(4, 4).Value, Location.Create(4, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(5, 5).Value, Location.Create(3, 1).Value];

        // Велосипедист, заказ X:совпадает, Y: ниже
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 3).Value, Location.Create(1, 1).Value, Location.Create(1, 1).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(5, 1).Value, Location.Create(5, 3).Value];

        // Велосипедист, заказ X:левее, Y: совпадает
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(3, 2).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(1, 5).Value, Location.Create(3, 5).Value];

        // Велосипедист, заказ X:левее, Y: ниже
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(3, 3).Value, Location.Create(1, 1).Value, Location.Create(1, 3).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(1, 1).Value, Location.Create(3, 5).Value];

        // Велосипедист, заказ ближе чем скорость
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(2, 1).Value, Location.Create(2, 1).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(5, 4).Value, Location.Create(5, 4).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(4, 5).Value, Location.Create(4, 5).Value];

        // Велосипедист, заказ с шагами по 2 осям
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(2, 2).Value, Location.Create(2, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(4, 4).Value, Location.Create(4, 4).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(1, 1).Value, Location.Create(1, 2).Value, Location.Create(1, 2).Value];
        yield return
            [Transport.Create("Bicycle",2).Value, Location.Create(5, 5).Value, Location.Create(5, 4).Value, Location.Create(5, 4).Value];
    }
    
    [Fact]
    public void DerivedEntity()
    {
        // Arrange
        // Act
        var isDerivedEntity = typeof(Transport).IsSubclassOf(typeof(Entity<Guid>));

        // Assert
        isDerivedEntity.Should().BeTrue();
    }
    
    [Fact]
    public void BeCorrectWhenParamsIsCorrect()
    {
        // Arrange
        // Act
        var result = Transport.Create("Pedestrian", 1);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be("Pedestrian");
    }
    
    [Theory]
    [MemberData(nameof(GetIncorrectTransportParams))]
    public void ReturnValueIsRequiredErrorWhenOrderIdIsEmpty(string name, int speed)
    {
        // Arrange
        // Act
        var result = Transport.Create(name, speed);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().NotBeNull();
    }

    [Theory]
    [MemberData(nameof(GetTransports))]
    public void ReturnCorrectIdAndName(string name, int speed)
    {
        // Arrange
        // Act
        var result = Transport.Create(name, speed);
        
        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be(name);
        result.Value.Speed.Should().Be(speed);
    }
    
    [Theory]
    [MemberData(nameof(GetTransportsAndLocations))]
    public void CanMove(Transport transport, Location currentLocation, Location targetLocation,
        Location locationAfterMove)
    {
        // Arrange
        // Act
        var result = transport.Move(currentLocation, targetLocation);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(locationAfterMove);
    }
}
