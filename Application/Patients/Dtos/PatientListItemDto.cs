namespace Application.Patients.Dtos;

/// <summary>
/// Lekkie DTO używane na liście pacjentów i w wynikach wyszukiwania.
/// Nie zawiera danych kontaktowych ani kolekcji – minimalna ilość danych do wyświetlenia wiersza na liście.
/// </summary>
public class PatientListItemDto
{
    public int Id { get; set; }
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }

    /// <summary>Liczba wszystkich wizyt pacjenta (nie kolekcja).</summary>
    public int VisitsCount { get; set; }
}
