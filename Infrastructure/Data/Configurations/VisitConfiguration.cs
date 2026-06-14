using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.Status)
            .IsRequired();

        builder.Property(v => v.ScheduledAt)
            .IsRequired();

        builder.Property(v => v.AssignedDoctorId)
            .HasMaxLength(450);

        builder.HasOne(v => v.Patient)
            .WithMany(p => p.Visits)
            .HasForeignKey(v => v.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(v => v.ProceduresPerformed)
            .WithOne()
            .HasForeignKey("VisitId")
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(v => v.ClinicalNotes)
            .WithOne()
            .HasForeignKey("VisitId")
            .OnDelete(DeleteBehavior.Cascade);

        // indeks na PatientId – GetByPatientIdAsync
        builder.HasIndex(v => v.PatientId)
            .HasDatabaseName("IX_Visits_PatientId");

        // indeks na AssignedDoctorId – GetByDoctorIdAsync
        builder.HasIndex(v => v.AssignedDoctorId)
            .HasDatabaseName("IX_Visits_AssignedDoctorId");

        // indeks na ScheduledAt – GetScheduledForDateAsync, sortowanie
        builder.HasIndex(v => v.ScheduledAt)
            .HasDatabaseName("IX_Visits_ScheduledAt");

        // indeks na Status – GetByStatusAsync
        builder.HasIndex(v => v.Status)
            .HasDatabaseName("IX_Visits_Status");
    }
}
