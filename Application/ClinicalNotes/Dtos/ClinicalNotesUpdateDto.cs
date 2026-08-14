namespace Application.ClinicalNotes.Dtos;

public class ClinicalNotesUpdateDto
{
    public string? MedicalHistory { get; set; }
    public string? Diagnosis { get; set; }
    public string? Recommendations { get; set; }
}