using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.Model.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

/// <summary>
///     Курьер
/// </summary>
public class Courier : Aggregate<Guid>
{
    /// <summary>
    ///     Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private Courier()
    {
    }

    /// <summary>
    ///     Ctr
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="transportType">Транспорт</param>
    /// <param name="location">Местоположение</param>
    private Courier(string name, TransportType transportType, Location location) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        TransportType = transportType;
        Location = location;
        Status = CourierStatus.Free;
    }

    /// <summary>
    ///     Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Вид транспорта
    /// </summary>
    public TransportType TransportType { get; private set; }

    /// <summary>
    ///     Геопозиция (X,Y)
    /// </summary>
    public Location Location { get; private set; }

    /// <summary>
    ///     Статус курьера
    /// </summary>
    public CourierStatus Status { get; private set; }

    /// <summary>
    ///     Factory Method
    /// </summary>
    /// <param name="name">Имя</param>
    /// <param name="transportType">Транспорт</param>
    /// <param name="location">Местоположение</param>
    /// <returns>Результат</returns>
    public static Result<Courier, Error> Create(string name, TransportType transportType, Location location)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (transportType == null) return GeneralErrors.ValueIsRequired(nameof(transportType));
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        return new Courier(name, transportType, location);
    }

    /// <summary>
    ///     Изменить местоположение
    /// </summary>
    /// <param name="targetLocation">Геопозиция</param>
    /// <returns>Результат</returns>
    public UnitResult<Error> Move(Location targetLocation)
    {
        if (targetLocation == null) return GeneralErrors.ValueIsRequired(nameof(targetLocation));

        var difX = targetLocation.X - Location.X;
        var difY = targetLocation.Y - Location.Y;

        var newX = Location.X;
        var newY = Location.Y;

        var cruisingRange = TransportType.Speed;

        if (difX > 0)
        {
            if (difX >= cruisingRange)
            {
                newX += cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difX < cruisingRange)
            {
                newX += difX;
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= difX;
            }
        }

        if (difX < 0)
        {
            if (Math.Abs(difX) >= cruisingRange)
            {
                newX -= cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difX) < cruisingRange)
            {
                newX -= Math.Abs(difX);
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
                cruisingRange -= Math.Abs(difX);
            }
        }

        if (difY > 0)
        {
            if (difY >= cruisingRange)
            {
                newY += cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (difY < cruisingRange)
            {
                newY += difY;
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        if (difY < 0)
        {
            if (Math.Abs(difY) >= cruisingRange)
            {
                newY -= cruisingRange;
                Location = Location.Create(newX, newY).Value;
                return UnitResult.Success<Error>();
            }

            if (Math.Abs(difY) < cruisingRange)
            {
                newY -= Math.Abs(difY);
                Location = Location.Create(newX, newY).Value;
                if (Location == targetLocation)
                    return UnitResult.Success<Error>();
            }
        }

        Location = Location.Create(newX, newY).Value;
        return UnitResult.Success<Error>();
    }

    /// <summary>
    ///     Сделать курьера занятым
    /// </summary>
    /// <returns>Результат</returns>
    public UnitResult<Error> SetBusy()
    {
        if (Status == CourierStatus.Busy) return Errors.TryAssignOrderWhenCourierHasAlreadyBusy();

        Status = CourierStatus.Busy;
        return UnitResult.Success<Error>();
    }

    /// <summary>
    ///     Сделать курьера свободным
    /// </summary>
    /// <returns>Результат</returns>
    public void SetFree()
    {
        Status = CourierStatus.Free;
    }

    /// <summary>
    ///     Рассчитать время до точки
    /// </summary>
    /// <param name="location">Конечное местоположение</param>
    /// <returns>Результат</returns>
    public Result<double, Error> CalculateTimeToLocation(Location location)
    {
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        var distanceToResult = Location.DistanceTo(location);
        if (distanceToResult.IsFailure) return distanceToResult.Error;
        var distance = distanceToResult.Value;

        var time = (double)distance / TransportType.Speed;
        return time;
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error TryAssignOrderWhenCourierHasAlreadyBusy()
        {
            return new Error($"{nameof(Courier).ToLowerInvariant()}.try.assign.order.when.courier.has.already.busy",
                "Нельзя взять заказ в работу, если курьер уже занят");
        }
    }
}