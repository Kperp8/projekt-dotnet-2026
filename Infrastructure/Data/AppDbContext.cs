using Infrastructure.Data.Configurations;
using Infrastructure.Identity;
using Domain.Procedures;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data;

public class AppDbContext : IdentityDbContext<AppUser, IdentityRole, string>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    public DbSet<Visit> Visits => Set<Visit>();
    public DbSet<MedicalRecord> MedicalRecords => Set<MedicalRecord>();
    public DbSet<MedicalDocument> MedicalDocuments => Set<MedicalDocument>();
    public DbSet<Procedure> Procedures => Set<Procedure>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // musi być wywołane jako pierwsze – konfiguruje tabele Identity
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new PatientConfiguration());
        builder.ApplyConfiguration(new VisitConfiguration());
        builder.ApplyConfiguration(new MedicalRecordConfiguration());
        builder.ApplyConfiguration(new ProceduresConfiguration());
    }
}
