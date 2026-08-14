using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class ClinicalNotesConfiguration : IEntityTypeConfiguration<ClinicalNote>
{
    public void Configure(EntityTypeBuilder<ClinicalNote> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.MedicalHistory)
        .IsRequired();
        
        builder.Property(c => c.Diagnosis)
        .IsRequired();

        builder.Property(c => c.Recommendations)
        .IsRequired();
    }
}