using Domain.Medications;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class MedicationConfiguration : IEntityTypeConfiguration<Medication>
{
    public void Configure(EntityTypeBuilder<Medication> builder)
    {
        builder.HasKey(m => m.Id);

        builder.Property(m => m.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(m => m.Dosing)
            .IsRequired()
            .HasMaxLength(350);

        builder.Property(m => m.Quantity)
            .IsRequired()
            .HasColumnType("int");

        builder.Property(m => m.Cost)
            .IsRequired()
            .HasColumnType("decimal(10,2)");
    }
}