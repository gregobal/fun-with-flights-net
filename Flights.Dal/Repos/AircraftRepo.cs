using Flights.Dal.EfStructures;
using Flights.Dal.Repos.Base;
using Flights.Dal.Repos.Interfaces;
using Flights.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace Flights.Dal.Repos;

public class AircraftRepo: BaseRepo<Aircraft, string?>, IAircraftRepo
{
    public AircraftRepo(FlightsDbContext context) : base(context)
    {
    }

    internal AircraftRepo(DbContextOptions<FlightsDbContext> options) : base(options)
    {
    }

    public override int Add(Aircraft entity, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public override int AddRange(IEnumerable<Aircraft> entities, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public override int Update(Aircraft entity, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public override int UpdateRange(IEnumerable<Aircraft> entities, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public override int Delete(Aircraft entity, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public override int DeleteRange(IEnumerable<Aircraft> entities, bool persist = true)
    {
        throw new InvalidOperationException();
    }

    public new void ExecuteQuery(string sql, object[] sqlParametersObjects)
    {
        throw new InvalidOperationException();
    }

    public new int SaveChanges()
    {
        throw new InvalidOperationException();
    }
}