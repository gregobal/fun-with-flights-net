namespace FlightsMinimalWebApi.Repository;

public class AircraftRepository: IAircraftRepository
{
    private readonly FlightsDbContext _context;

    public AircraftRepository(FlightsDbContext context)
    {
        _context = context;
    }

    public async Task<List<AircraftsDatum>> GetAircraftsAsync() =>
        await _context.AircraftsData.ToListAsync();

    public async Task<AircraftsDatum?> GetAircraftAsync(string code) =>
        await _context.AircraftsData.FirstOrDefaultAsync(a => a.AircraftCode == code);

    public async Task CreateAircraftAsync(AircraftsDatum aircraft) =>
        await _context.AircraftsData.AddAsync(aircraft);

    public async Task UpdateAircraftAsync(AircraftsDatum aircraft)
    {
        var founded = await _context.AircraftsData.FindAsync(aircraft.AircraftCode);
        if (founded is null) return;
        founded.Model = aircraft.Model;
        founded.Range = aircraft.Range;
    }

    public async Task DeleteAircraftAsync(string code)
    {
        var founded = await _context.AircraftsData.FindAsync(code);
        if (founded is null) return;
        _context.AircraftsData.Remove(founded);
    }

    public async Task SaveAsync() => await _context.SaveChangesAsync();

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }

        _disposed = true;
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}