using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MedicalRecordConfiguration : IEntityTypeConfiguration<MedicalRecord>
{
    public void Configure(EntityTypeBuilder<MedicalRecord> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.BloodType)
            .IsRequired()
            .HasMaxLength(10);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        // ICollection<string> przechowywane jako JSON
        builder.Property(m => m.Allergies)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasColumnType("nvarchar(max)");

        builder.Property(m => m.ChronicDiseases)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasColumnType("nvarchar(max)");

        builder.Property(m => m.Notes)
            .HasConversion(
                v => string.Join('|', v),
                v => v.Split('|', StringSplitOptions.RemoveEmptyEntries).ToList())
            .HasColumnType("nvarchar(max)");

        builder.HasOne(m => m.Patient)
            .WithMany()
            .HasForeignKey(m => m.PatientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(m => m.MedicalDocuments)
            .WithOne(d => d.MedicalRecord)
            .HasForeignKey(d => d.MedicalRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // indeks na PatientId – GetByPatientIdAsync
        builder.HasIndex(m => m.PatientId)
            .HasDatabaseName("IX_MedicalRecords_PatientId");

        // indeks na BloodType – GetByBloodTypeAsync
        builder.HasIndex(m => m.BloodType)
            .HasDatabaseName("IX_MedicalRecords_BloodType");

        // indeks na CreatedAt – GetByCreationDateAsync, sortowanie
        builder.HasIndex(m => m.CreatedAt)
            .HasDatabaseName("IX_MedicalRecords_CreatedAt");
    }
}