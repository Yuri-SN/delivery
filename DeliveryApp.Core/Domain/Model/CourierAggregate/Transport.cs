using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

/// <summary>
///     Транспорт
/// </summary>
public class Transport : Entity<Guid>
{
    /// <summary>
    ///     Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private Transport()
    {
    }

    /// <summary>
    ///     Ctr
    /// </summary>
    private Transport(string name, int speed) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Speed = speed;
    }

    /// <summary>
    ///     Factory Method
    /// </summary>
    /// <param name="name">Название транспорта</param>
    /// <param name="speed">Скорость транспорта</param>
    /// <returns>Результат</returns>
    public static Result<Transport, Error> Create(string name, int speed)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (speed is < 1 or > 3) return GeneralErrors.ValueIsRequired(nameof(speed));

        return new Transport(name, speed);
    }

    /// <summary>
    ///     Название
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Скорость
    /// </summary>
    public int Speed { get; }

    /// <summary>
    ///     Изменить местоположение
    /// </summary>
    /// <param name="current">Текущее местоположение</param>
    /// <param name="target">Целевое местоположение</param>
    /// <returns>Местоположение после сдвига</returns>
    public Result<Location, Error> Move(Location current, Location target)
    {
        if (current == null) return GeneralErrors.ValueIsRequired(nameof(current));
        if (target == null) return GeneralErrors.ValueIsRequired(nameof(target));

        var difX = target.X - current.X;
        var difY = target.Y - current.Y;
        var cruisingRange = Speed;

        var moveX = Math.Clamp(difX, -cruisingRange, cruisingRange);
        cruisingRange -= Math.Abs(moveX);

        var moveY = Math.Clamp(difY, -cruisingRange, cruisingRange);
        var newLocation = Location.Create(current.X + moveX, current.Y + moveY).Value;

        return newLocation;
    }
}