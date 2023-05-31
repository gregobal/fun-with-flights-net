namespace FlightsMinimalWebApi.DAL;

public interface IAircraftRepository: IDisposable
{
    Task<List<Aircraft>> GetAircraftsAsync();
    Task<Aircraft?> GetAircraftAsync(string code);
    Task CreateAircraftAsync(Aircraft aircraft);
    Task UpdateAircraftAsync(Aircraft aircraft);
    Task DeleteAircraftAsync(string code);
    Task SaveAsync();
}