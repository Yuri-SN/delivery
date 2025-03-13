using System.Linq;
using System.Reflection;
using DeliveryApp.Core.Domain.Model.CourierAggregate;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using FluentAssertions;
using Primitives;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.CourierAggregate;

public class CourierShould
{
    [Fact]
    public void ConstructorShouldBePrivate()
    {
        // Arrange
        var typeInfo = typeof(Courier).GetTypeInfo();

        // Act

        // Assert
        typeInfo.DeclaredConstructors.All(x => x.IsPrivate).Should().BeTrue();
    }

    [Fact]
    public void BeCorrectWhenParamsIsCorrect()
    {
        // Arrange
        // Act
        var result = Courier.Create("Ваня", "Pedestrian", 1, Location.MinLocation);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().NotBeEmpty();
        result.Value.Name.Should().Be("Ваня");
        result.Value.Transport.Name.Should().Be("Pedestrian");
        result.Value.Transport.Speed.Should().Be(1);
        result.Value.Location.Should().Be(Location.MinLocation);
    }

    [Fact]
    public void ReturnValueIsRequiredErrorWhenNameIsEmpty()
    {
        // Arrange
        // Act
        var result = Courier.Create("", "Pedestrian", 1, Location.MinLocation);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(GeneralErrors.ValueIsRequired("name"));
    }

    [Fact]
    public void ChangeLocationAfterMove()
    {
        // Arrange
        var courier = Courier.Create("Ваня", "Pedestrian", 1, Location.MinLocation).Value;
        var targetLcation = Location.Create(2, 1).Value;
        
        // Act
        var result = courier.Move(Location.MaxLocation);

        // Assert
        result.IsSuccess.Should().BeTrue();
        courier.Location.Should().Be(targetLcation);
    }

    [Fact]
    public void CantMoveToIncorrectLocation()
    {
        // Arrange
        var courier = Courier.Create("Ваня", "Pedestrian", 1, Location.MinLocation).Value;

        // Act
        var result = courier.Move(null);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().BeEquivalentTo(GeneralErrors.ValueIsRequired("target"));
    }

    [Fact]
    public void CanCalculateTimeToLocation()
    {
        /*
        Изначальная точка курьера: [1,1]
        Целевая точка: [5,10]
        Количество шагов, необходимое курьеру: 13 (4 по горизонтали и 9 по вертикали)
        Скорость транспорта (пешехода): 1 шаг в 1 такт
        Время подлета: 13/13 = 13.0 тактов потребуется курьеру, чтобы доставить заказ
        */

        // Arrange
        var location = Location.Create(5, 10).Value;
        var courier = Courier.Create("Ваня", "Pedestrian", 1, Location.MinLocation).Value;

        // Act
        var result = courier.CalculateTimeToLocation(location);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(13);
    }
}
