namespace Application.ClinicalNotes.Dtos;

//// <summary>Lekkie DTO używane na liście notatek klinicznych.</summary>
public class ClinicalNotesListItemDto
{
    public int Id { get; set; }
    public string Diagnosis { get; set; } = string.Empty;
}