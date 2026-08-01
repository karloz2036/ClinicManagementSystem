using ClinicManagementSystem.Api.ExceptionHandling;
using ClinicManagementSystem.Application.Features.Genders.Interfaces;
using ClinicManagementSystem.Application.Features.Genders.Services;
using ClinicManagementSystem.Application.Features.Patients.Interfaces;
using ClinicManagementSystem.Application.Features.Patients.Services;
using ClinicManagementSystem.Application.Features.Specialties.Interfaces;
using ClinicManagementSystem.Application.Features.Specialties.Services;
using ClinicManagementSystem.Application.Features.ConsultingRooms.Interfaces;
using ClinicManagementSystem.Application.Features.ConsultingRooms.Services;
using ClinicManagementSystem.Application.Features.Doctors.Interfaces;
using ClinicManagementSystem.Application.Features.Doctors.Services;
using ClinicManagementSystem.Application.Features.AppointmentStatuses.Interfaces;
using ClinicManagementSystem.Application.Features.AppointmentStatuses.Services;
using ClinicManagementSystem.Application.Features.Appointments.Interfaces;
using ClinicManagementSystem.Application.Features.Appointments.Services;
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

builder.Services.AddScoped<ISpecialtyRepository, SpecialtyRepository>();
builder.Services.AddScoped<ISpecialtyService, SpecialtyService>();
builder.Services.AddScoped<IConsultingRoomRepository, ConsultingRoomRepository>();
builder.Services.AddScoped<IConsultingRoomService, ConsultingRoomService>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentStatusRepository, AppointmentStatusRepository>();
builder.Services.AddScoped<IAppointmentStatusService, AppointmentStatusService>();
builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();


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
