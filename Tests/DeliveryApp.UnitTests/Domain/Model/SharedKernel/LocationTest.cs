using DeliveryApp.Core.Domain.Model.SharedKernel;
using FluentAssertions;
using Xunit;

namespace DeliveryApp.UnitTests.Domain.Model.SharedKernel;

public class LocationShould
{
    [Fact]
    public void BeCorrectWhenParamsIsCorrectOnCreated()
    {
        // Arrange

        // Act
        var location = Location.Create(3, 5);

        // Assert
        location.IsSuccess.Should().BeTrue();
        location.Value.X.Should().Be(3);
        location.Value.Y.Should().Be(5);
    }
    
    [Theory]
    [InlineData(0, 0)]
    [InlineData(21, 15)]
    public void ReturnErrorWhenParamsIsInCorrectOnCreated(byte x, byte y)
    {
        // Arrange

        // Act
        var location = Location.Create(x, y);

        //Assert
        location.IsSuccess.Should().BeFalse();
        location.Error.Should().NotBeNull();
    }

    [Fact]
    public void BeEqualWhenAllPropertiesIsEqual()
    {
        // Arrange
        var first = Location.Create(3, 6).Value;
        var second = Location.Create(3, 6).Value;

        // Act
        var result = first == second;

        // Assert
        result.Should().BeTrue();
    }
    
    [Fact]
    public void BeNotEqualWhenAllPropertiesIsEqual()
    {
        // Arrange
        var first = Location.Create(3, 8).Value;
        var second = Location.Create(8, 3).Value;

        // Act
        var result = first == second;

        // Assert
        result.Should().BeFalse();
    }
}
