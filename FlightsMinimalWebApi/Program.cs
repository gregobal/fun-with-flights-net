using System.ComponentModel.DataAnnotations.Schema;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FlightsDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FlightsDb>();
}

app.MapGet("/aircrafts", async (FlightsDb db) => await db.Aircrafts.ToListAsync());

app.MapGet("/aircrafts/{code}", async (string code, FlightsDb db) =>
    await db.Aircrafts.FirstOrDefaultAsync(a => a.Code == code) is Aircraft aircraft
        ? Results.Ok(aircraft)
        : Results.NotFound());

app.MapPost("/aircrafts", async ([FromBody] Aircraft aircraft, FlightsDb db) =>
{
    db.Aircrafts.Add(aircraft);
    await db.SaveChangesAsync();
    return Results.Created($"/aircrafts/{aircraft.Code}", aircraft);
});

app.MapPut("/aircrafts",
    async ([FromBody] Aircraft aircraft, FlightsDb db) =>
    {
        var founded = db.Aircrafts.FindAsync(aircraft.Code).Result;
        if (founded is null)
            return Results.NotFound();
        founded.Model = aircraft.Model;
        founded.Range = aircraft.Range;
        await db.SaveChangesAsync();
        return Results.NoContent();
    });

app.MapDelete("/aircrafts/{code}", async (string code, FlightsDb db) =>
{
    var founded = db.Aircrafts.FindAsync(code).Result;
    if (founded is null)
        return Results.NotFound();
    db.Aircrafts.Remove(founded);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();

public class FlightsDb : DbContext
{
    public FlightsDb(DbContextOptions<FlightsDb> options) :
        base(options)
    {
    }

    public DbSet<Aircraft> Aircrafts => Set<Aircraft>();
}

[Table("aircrafts_data")]
public class Aircraft
{
    [Key] [Column("aircraft_code")] public string Code { get; set; } = "000";
    [Column("model", TypeName = "jsonb")] public string Model { get; set; } = String.Empty;
    [Column("range")] public int Range { get; set; }
}