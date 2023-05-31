var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FlightsDb>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FlightsDb>();
}

app.MapGet("/aircrafts", async (IAircraftRepository repository) => 
    Results.Ok(await repository.GetAircraftsAsync()));

app.MapGet("/aircrafts/{code}", async (string code, IAircraftRepository repository) =>
    await repository.GetAircraftAsync(code) is Aircraft aircraft
        ? Results.Ok(aircraft)
        : Results.NotFound());

app.MapPost("/aircrafts", async ([FromBody] Aircraft aircraft, IAircraftRepository repository) =>
{
    await repository.CreateAircraftAsync(aircraft);
    await repository.SaveAsync();
    return Results.Created($"/aircrafts/{aircraft.Code}", aircraft);
});

app.MapPut("/aircrafts",
    async ([FromBody] Aircraft aircraft, IAircraftRepository repository) =>
    {
        await repository.UpdateAircraftAsync(aircraft);
        await repository.SaveAsync();
        return Results.NoContent();
    });

app.MapDelete("/aircrafts/{code}", async (string code, IAircraftRepository repository) =>
{
    await repository.DeleteAircraftAsync(code);
    await repository.SaveAsync();
    return Results.NoContent();
});

app.UseHttpsRedirection();

app.Run();