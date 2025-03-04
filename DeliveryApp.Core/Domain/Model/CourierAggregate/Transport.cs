using System.Diagnostics.CodeAnalysis;
using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.CourierAggregate;

/// <summary>
///     Вид транспорта курьера
/// </summary>
public class Transport : Entity<int>
{
    private const int PedestrianSpeed = 1;
    private const int BicycleSpeed = 2;
    private const int CarSpeed = 3;

    public static readonly Transport Pedestrian = new(1, nameof(Pedestrian).ToLowerInvariant(), PedestrianSpeed);
    public static readonly Transport Bicycle = new(2, nameof(Bicycle).ToLowerInvariant(), BicycleSpeed);
    public static readonly Transport Car = new(3, nameof(Car).ToLowerInvariant(), CarSpeed);

    /// <summary>
    ///     Название транспорта
    /// </summary>
    public string Name { get; }

    /// <summary>
    ///     Скорость
    /// </summary>
    public int Speed { get; private set; }

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
    /// <param name="id">Идентификатор</param>
    /// <param name="name">Название транспорта</param>
    /// <param name="speed">Скорость</param>
    private Transport(int id, string name, int speed) : this()
    {
        Id = id;
        Name = name;
        Speed = speed;
    }
    
    /// <summary>
    ///     Список всех видов транспорта
    /// </summary>
    /// <returns>Названия транспортов</returns>
    public static IEnumerable<Transport> List()
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
    public static Result<Transport, Error> FromName(string name)
    {
        var state = List()
            .SingleOrDefault(s => string.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));
        if (state == null) return Errors.TransportIsWrong();
        return state;
    }

    /// <summary>
    ///     Получить транспорт по идентификатору
    /// </summary>
    /// <param name="id">Идентификатор</param>
    /// <returns>Транспорт</returns>
    public static Result<Transport, Error> FromId(int id)
    {
        var state = List().SingleOrDefault(s => s.Id == id);
        if (state == null) return Errors.TransportIsWrong();
        return state;
    }
    
    /// <summary>
    ///     Ошибки, которые может возвращать сущность
    /// </summary>
    public static class Errors
    {
        public static Error TransportIsWrong()
        {
            return new Error($"{nameof(Transport).ToLowerInvariant()}.is.wrong",
                $"Не верное значение. Допустимые значения: {nameof(Transport).ToLowerInvariant()}: {string.Join(",", List().Select(s => s.Name))}");
        }
    }
}
