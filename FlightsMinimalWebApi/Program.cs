using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var aircrafts = new List<AircraftsData>();

app.MapGet("/aircrafts", () => aircrafts);
app.MapGet("/aircrafts/{code}", 
    (string code) => aircrafts.FirstOrDefault(a => a.AircraftCode == code));
app.MapPost("/aircrafts", (AircraftsData aircraft) => aircrafts.Add(aircraft));
app.MapPut("/aircrafts", (AircraftsData aircraft) =>
{
    var idx = aircrafts.FindIndex(a => a.AircraftCode == aircraft.AircraftCode);
    if (idx < 0)
        throw new Exception("Not found");
    aircrafts[idx] = aircraft;
});
app.MapDelete("/aircrafts/{code}", (string code) =>
{
    var idx = aircrafts.FindIndex(a => a.AircraftCode == code);
    if (idx < 0)
        throw new Exception("Not found");
    aircrafts.RemoveAt(idx);
});

app.Run();

public class AircraftsData
{
    public string AircraftCode { get; set; } = String.Empty;
    public string Model { get; set; } = String.Empty;
    public int Range { get; set; }
}
