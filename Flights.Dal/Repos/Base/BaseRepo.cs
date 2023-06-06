using Flights.Dal.EfStructures;
using Flights.Dal.Exceptions;
using Flights.Models.Entities.Base;
using Microsoft.EntityFrameworkCore;

namespace Flights.Dal.Repos.Base;

public abstract class BaseRepo<T, TP> : IRepo<T, TP> where T : BaseEntity<TP>, new()
{
    private readonly bool _disposeContext;
    public FlightsDbContext Context { get; }
    public DbSet<T> Table { get; }

    protected BaseRepo(FlightsDbContext context)
    {
        Context = context;
        Table = Context.Set<T>();
        _disposeContext = false;
    }

    protected BaseRepo(DbContextOptions<FlightsDbContext> options)
        : this(new FlightsDbContext(options))
    {
        _disposeContext = true;
    }

    public virtual IEnumerable<T> GetAll() => Table;

    public virtual IEnumerable<T> GetAllIgnoreQueryFilters() => Table.IgnoreQueryFilters();
    
    public virtual T? Find(TP key) => Table.Find(key);

    public virtual T? FindAsNoTracking(TP key) => Table.AsNoTrackingWithIdentityResolution()
        .FirstOrDefault(x => Equals(x.Key, key));

    public virtual T? FindIgnoreQueryFilters(TP key) => Table.IgnoreQueryFilters()
        .FirstOrDefault(x => Equals(x.Key, key));

    public void ExecuteQuery(string sql, object[] sqlParametersObjects) => Context.Database
        .ExecuteSqlRaw(sql, sqlParametersObjects);

    public virtual int Add(T entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int AddRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.AddRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public virtual int Update(T entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int UpdateRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.UpdateRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public virtual int Delete(T entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChanges() : 0;
    }

    public virtual int DeleteRange(IEnumerable<T> entities, bool persist = true)
    {
        Table.RemoveRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public int SaveChanges()
    {
        try
        {
            return Context.SaveChanges();
        }
        catch (CustomException ex)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new CustomException("An error occured updating database", ex);
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed) return;
        if (disposing)
        {
            if (_disposeContext)
            {
                Context.Dispose();
            }
        }

        _disposed = true;
    }

    ~BaseRepo()
    {
        Dispose(false);
    }
}