namespace Application.ClinicalNotes.Dtos;

//// <summary>Pełne DTO notatki zwracane przez GET /clinicalNotes/{id}.</summary>
public class ClinicalNotesDetailsDto
{
    public int Id { get; set; }
    public string MedicalHistory { get; set; } = string.Empty;
    public string Diagnosis { get; set; } = string.Empty;
    public string Recommendations { get; set; } = string.Empty;
}