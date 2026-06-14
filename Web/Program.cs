using Application.MedicalRecords.Interfaces;
using Application.MedicalRecords.Mappers;
using Application.MedicalRecords.Services;
using Application.Patients.Interfaces;
using Application.Patients.Mappers;
using Application.Patients.Services;
using Application.Visits.Interfaces;
using Application.Visits.Mappers;
using Application.Visits.Services;
using Domain.MedicalRecords;
using Domain.Patients;
using Domain.Visits;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// --- Baza danych ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// --- Identity ---
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequiredLength = 8;
        options.Password.RequireUppercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// --- Serwisy aplikacyjne ---
builder.Services.AddScoped<IPatientRepository, PatientRepository>();
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<PatientMapper>();
builder.Services.AddScoped<IVisitsRepository, VisitRepository>();
builder.Services.AddScoped<IVisitService, VisitService>();
builder.Services.AddScoped<VisitMapper>();
builder.Services.AddScoped<IMedicalRecordsRepository, MedicalRecordRepository>();
builder.Services.AddScoped<IMedicalRecordService, MedicalRecordService>();
builder.Services.AddScoped<MedicalRecordMapper>();

// --- Razor Pages + Controllers (potrzebne pod API) ---
builder.Services.AddRazorPages();
builder.Services.AddControllers();

var app = builder.Build();

// --- Middleware ---
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// UseAuthentication musi być przed UseAuthorization
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();
app.MapControllers();

app.Run();
