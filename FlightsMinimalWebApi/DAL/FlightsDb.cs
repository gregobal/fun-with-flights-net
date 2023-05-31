public class FlightsDb : DbContext
{
    public FlightsDb(DbContextOptions<FlightsDb> options) :
        base(options)
    {
    }

    public DbSet<Aircraft> Aircrafts => Set<Aircraft>();
}