using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configurations;

public class PatientConfiguration : IEntityTypeConfiguration<Patient>
{
    public void Configure(EntityTypeBuilder<Patient> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Pesel)
            .IsRequired()
            .HasMaxLength(11);

        builder.Property(p => p.FirstName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.LastName)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.InsuranceNumber)
            .HasMaxLength(20);

        builder.Property(p => p.PhoneNumber)
            .HasMaxLength(20);

        builder.Property(p => p.Email)
            .HasMaxLength(200);

        builder.Property(p => p.Address)
            .HasMaxLength(300);

        // indeks unikalny na PESEL – wymaganie specyfikacji (optymalizacja + integralność)
        builder.HasIndex(p => p.Pesel)
            .IsUnique()
            .HasDatabaseName("IX_Patients_Pesel");

        // indeks na LastName – wyszukiwanie pacjentów po nazwisku
        builder.HasIndex(p => p.LastName)
            .HasDatabaseName("IX_Patients_LastName");

        // globalny filtr – automatycznie wyklucza usuniętych pacjentów ze wszystkich zapytań
        builder.HasQueryFilter(p => !p.IsDeleted);
    }
}
