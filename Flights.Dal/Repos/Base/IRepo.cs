namespace Flights.Dal.Repos.Base;

public interface IRepo<T>: IDisposable
{
    int Add(T entity, bool persist = true);
    int AddRange(IEnumerable<T> entities, bool persist = true);
    int Update(T entity, bool persist = true);
    int UpdateRange(IEnumerable<T> entities, bool persist = true);
    int Delete(T entity, bool persist = true);
    int DeleteRange(IEnumerable<T> entities, bool persist = true);
    T? Find(string id);
    T? FindAsNoTracking(string id);
    T? FindIgnoreQueryFilters(string id);
    IEnumerable<T> GetAll();
    IEnumerable<T> GetAllIgnoreQueryFilters();
    void ExecuteQuery(string sql, object[] sqlParametersObjects);
    int SaveChanges();
}