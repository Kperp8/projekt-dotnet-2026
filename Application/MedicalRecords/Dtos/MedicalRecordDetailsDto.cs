namespace Application.MedicalRecords.Dtos;

/// <summary>Pełne DTO rekordu medycznego zwracane przez GET /records/{id}.</summary>
public class MedicalRecordDetailsDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;
    public ICollection<string> Allergies { get; set; } = new List<string>();
    public ICollection<string> ChronicDiseases { get; set; } = new List<string>();
    public ICollection<string> Notes { get; set; } = new List<string>();
    public DateTime CreatedAt { get; set; }
    public IReadOnlyList<MedicalDocumentDto> Documents { get; set; } = [];
}