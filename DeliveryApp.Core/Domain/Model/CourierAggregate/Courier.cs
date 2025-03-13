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
    /// <param name="transport">Транспорт</param>
    /// <param name="location">Местоположение</param>
    private Courier(string name, Transport transport, Location location) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        Transport = transport;
        Location = location;
        Status = CourierStatus.Free;
    }

    /// <summary>
    ///     Имя
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    ///     Транспорт
    /// </summary>
    public Transport Transport { get; private set; }

    /// <summary>
    ///     Геопозиция (X, Y)
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
    /// <param name="transportName">Название транспорта</param>
    /// <param name="transportSpeed">Скорость транспорта</param>
    /// <param name="location">Местоположение</param>
    /// <returns>Результат</returns>
    public static Result<Courier, Error> Create(string name, string transportName, int transportSpeed,
        Location location)
    {
        if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
        if (string.IsNullOrEmpty(transportName)) return GeneralErrors.ValueIsRequired(nameof(transportName));
        if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

        var (_, isFailure, transport, error) = Transport.Create(transportName, transportSpeed);
        if (isFailure) return error;

        return new Courier(name, transport, location);
    }

    /// <summary>
    ///     Изменить местоположение
    /// </summary>
    /// <param name="target">Геопозиция</param>
    /// <returns>Результат</returns>
    public UnitResult<Error> Move(Location target)
    {
        if (target == null) return GeneralErrors.ValueIsRequired(nameof(target));

        var transportMoveResult = Transport.Move(Location, target);
        if (transportMoveResult.IsFailure)
        {
            return transportMoveResult.Error;
        }

        Location = transportMoveResult.Value;

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

        var time = (double)distance / Transport.Speed;
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