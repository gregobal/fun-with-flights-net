namespace Flights.Models.Entities.Base;

public abstract class BaseEntity<T>
{
    public abstract T Key { get; }
}