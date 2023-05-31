namespace FlightsMinimalWebApi.DAL;

public class AircraftRepository: IAircraftRepository
{
    private readonly FlightsDb _context;

    public AircraftRepository(FlightsDb context)
    {
        _context = context;
    }

    public async Task<List<Aircraft>> GetAircraftsAsync() =>
        await _context.Aircrafts.ToListAsync();

    public async Task<Aircraft?> GetAircraftAsync(string code) =>
        await _context.Aircrafts.FirstOrDefaultAsync(a => a.Code == code);

    public async Task CreateAircraftAsync(Aircraft aircraft) =>
        await _context.Aircrafts.AddAsync(aircraft);

    public async Task UpdateAircraftAsync(Aircraft aircraft)
    {
        var founded = await _context.Aircrafts.FindAsync(aircraft.Code);
        if (founded is null) return;
        founded.Model = aircraft.Model;
        founded.Range = aircraft.Range;
    }

    public async Task DeleteAircraftAsync(string code)
    {
        var founded = await _context.Aircrafts.FindAsync(code);
        if (founded is null) return;
        _context.Aircrafts.Remove(founded);
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