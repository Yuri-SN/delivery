using CSharpFunctionalExtensions;
using System.Diagnostics.CodeAnalysis;
using Primitives;

namespace DeliveryApp.Core.Domain.Model.SharedKernel;

/// <summary>
///     Геопозиция
/// </summary>
public class Location : ValueObject
{
    /// <summary>
    ///     Ctr
    /// </summary>
    [ExcludeFromCodeCoverage]
    private Location()
    {
    }

    /// <summary>
    ///     Ctr
    /// </summary>
    /// <param name="x">Горизонталь</param>
    /// <param name="y">Вертикаль</param>
    private Location(byte x, byte y) : this()
    {
        X = x;
        Y = y;
    }

    /// <summary>
    ///     Горизонталь
    /// </summary>
    public byte X { get; private set;}

    /// <summary>
    ///     Вертикаль
    /// </summary>
    public byte Y { get; private set;}

    /// <summary>
    ///     Factory Method
    /// </summary>
    /// <param name="x">Горизонталь</param>
    /// <param name="y">Вертикаль</param>
    /// <returns>Результат</returns>
    public static Result<Location, Error> Create(byte x, byte y)
    {
        if (x is < 1 or > 10 ) return GeneralErrors.ValueIsInvalid(nameof(x));
        if (y is < 1 or > 10 ) return GeneralErrors.ValueIsInvalid(nameof(y));
        return new Location(x, y);
    }
    
    /// <summary>
    ///     Расстояние до другой локации
    /// </summary>
    /// <param name="other">Локация</param>
    /// <returns>Результат</returns>
    public byte CalculateDistance(Location other)
    {
        return (byte)(Math.Abs(X - other.X) + Math.Abs(Y - other.Y));
    }
    
    /// <summary>
    ///     Создание случайной локации
    /// </summary>
    /// <returns>Результат</returns>
    public static Location CreateRandomLocation()
    {
        var random = new Random();
        return new Location((byte)(random.Next(1, 11)), (byte)(random.Next(1, 11)));
    }

    /// <summary>
    ///     Перегрузка для определения идентичности
    /// </summary>
    /// <returns>Результат</returns>
    /// <remarks>Идентичность будет происходить по совокупности полей указанных в методе</remarks>
    [ExcludeFromCodeCoverage]
    protected override IEnumerable<IComparable> GetEqualityComponents()
    {
        yield return X;
        yield return Y;
    }
}
