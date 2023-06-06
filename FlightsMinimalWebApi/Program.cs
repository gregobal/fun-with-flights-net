
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddDbContext<FlightsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("Postgres"));
});

builder.Services.AddSingleton<ITokenService, TokenService>();

builder.Services.AddSingleton<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAircraftRepository, AircraftRepository>();

builder.Services.AddAuthorization();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
        };
    });

var app = builder.Build();

app.UseAuthentication();

app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FlightsDbContext>();
}

app.MapGet("/login", [AllowAnonymous]
    async ([FromQuery]string login, [FromQuery]string pass, 
        ITokenService tokenService, IUserRepository userRepository) =>
    {
        UserModel userModel = new()
        {
            Login = login,
            Password = pass
        };
        var userDto = userRepository.GetUser(userModel);
        if (userDto is null) return Results.Unauthorized();
        var token = tokenService.BuildToken(builder.Configuration["Jwt:Key"]!,
            builder.Configuration["Jwt:Issuer"]!, userDto);
        return Results.Ok(token);
    });

app.MapGet("/aircrafts", async (IAircraftRepository repository) =>
        Results.Ok(await repository.GetAircraftsAsync()))
    .WithTags("Read");

app.MapGet("/aircrafts/{code}", async (string code, IAircraftRepository repository) =>
        await repository.GetAircraftAsync(code) is AircraftsDatum aircraft
            ? Results.Ok(aircraft)
            : Results.NotFound())
    .WithTags("Read");

app.MapPost("/aircrafts", [Authorize] async ([FromBody] AircraftsDatum aircraft, IAircraftRepository repository) =>
{
    await repository.CreateAircraftAsync(aircraft);
    await repository.SaveAsync();
    return Results.Created($"/aircrafts/{aircraft.AircraftCode}", aircraft);
}).WithTags("Mutate");

app.MapPut("/aircrafts", [Authorize] async ([FromBody] AircraftsDatum aircraft, IAircraftRepository repository) =>
{
    await repository.UpdateAircraftAsync(aircraft);
    await repository.SaveAsync();
    return Results.NoContent();
}).WithTags("Mutate");

app.MapDelete("/aircrafts/{code}", [Authorize] async (string code, IAircraftRepository repository) =>
{
    await repository.DeleteAircraftAsync(code);
    await repository.SaveAsync();
    return Results.NoContent();
}).WithTags("Mutate");

app.UseHttpsRedirection();

app.Run();