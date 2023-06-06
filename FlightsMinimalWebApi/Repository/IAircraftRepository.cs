namespace FlightsMinimalWebApi.Repository;

public interface IAircraftRepository: IDisposable
{
    Task<List<AircraftsDatum>> GetAircraftsAsync();
    Task<AircraftsDatum?> GetAircraftAsync(string code);
    Task CreateAircraftAsync(AircraftsDatum aircraft);
    Task UpdateAircraftAsync(AircraftsDatum aircraft);
    Task DeleteAircraftAsync(string code);
    Task SaveAsync();
}