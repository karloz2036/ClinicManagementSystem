using ClinicManagementSystem.Api.ExceptionHandling;
using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using ClinicManagementSystem.Application.Features.Genders.Services;
using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Application.Features.Patients.Services;
using ClinicManagementSystem.Infrastructure.Data;
using ClinicManagementSystem.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ClinicDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddScoped<IGenderRepository, GenderRepository>();
builder.Services.AddScoped<IGenderService, GenderService>();

builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();


builder.Services.AddControllers(); // Add this line to register controllers

//lineas para el manejador global de excepciones.
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();

//lineas para el manejador global de excepciones.
app.UseExceptionHandler();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers(); // Add this line to map controller routes

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.MapGet("/api/database-test", async (ClinicDbContext dbContext) =>
{
    var canConnect = await dbContext.Database.CanConnectAsync(); 
    return Results.Ok(new
    {
        databaseConnected = canConnect
    });
});

app.MapGet("/api/genders-test", async (ClinicDbContext dbContext) =>
{
    var genders = await dbContext.Genders
        .AsNoTracking()
        .Select(g => new
        {
            g.Id,
            g.Name,
            g.IsActive
        })
        .ToListAsync();

    return Results.Ok(genders);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
