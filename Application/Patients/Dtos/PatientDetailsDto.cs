namespace Application.Patients.Dtos;

/// <summary>
/// Pełne DTO pacjenta zwracane przez GET /api/patients/{id}.
/// Zawiera wszystkie dane kontaktowe. IsDeleted i inne pola techniczne są pominięte celowo.
/// </summary>
public class PatientDetailsDto
{
    public int Id { get; set; }
    public string Pesel { get; set; } = string.Empty;
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime BirthDate { get; set; }
    public string InsuranceNumber { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public int VisitsCount { get; set; }
}
