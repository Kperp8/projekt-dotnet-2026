using Infrastructure.Data.Configurations;
using Infrastructure.Identity;
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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        // musi być wywołane jako pierwsze – konfiguruje tabele Identity
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new PatientConfiguration());
        builder.ApplyConfiguration(new VisitConfiguration());
    }
}
