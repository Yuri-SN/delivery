using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

/// <summary>
///     Транспорт
/// </summary>
public class TransportType : Entity<int>
{
    public static readonly TransportType Pedestrian = new(1, nameof(Pedestrian).ToLowerInvariant(), 1);
    public static readonly TransportType Bicycle = new(2, nameof(Bicycle).ToLowerInvariant(), 2);
    public static readonly TransportType Car = new(3, nameof(Car).ToLowerInvariant(), 3);

    /// <summary>
    ///     Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private TransportType()
    {
    }

    /// <summary>
    ///     Ctr
    /// </summary>
    private TransportType(int id, string name, int speed) : this()
    {
        Id = id;
        Name = name;
        Speed = speed;
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
    ///     Список всех значений списка
    /// </summary>
    /// <returns>Значения списка</returns>
    public static IEnumerable<TransportType> List()
    {
        yield return Pedestrian;
        yield return Bicycle;
        yield return Car;
    }

    /// <summary>
    ///     Получить транспорт по названию
    /// </summary>
    /// <param name="name">Название</param>
    /// <returns>Транспорт</returns>
    public static Result<TransportType, Error> FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null) return Errors.StatusIsWrong();
        return state;
    }

    /// <summary>
    ///     Получить транспорт по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Транспорт</returns>
    public static Result<TransportType, Error> FromId(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null) return Errors.StatusIsWrong();
        return state;
    }

    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class Errors
    {
        public static Error StatusIsWrong()
        {
            return new Error($"{nameof(TransportType).ToLowerInvariant()}.is.wrong",
                $"Не верное значение. Допустимые значения: {nameof(TransportType).ToLowerInvariant()}: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}