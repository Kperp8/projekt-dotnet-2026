using System.ComponentModel.DataAnnotations;

namespace Application.ClinicalNotes.Dtos;

public class ClinicalNotesCreateDto
{
    [Required(ErrorMessage = "Historia/wywiad jest wymagana.")]
    public string MedicalHistory { get; set; } = string.Empty;

    [Required(ErrorMessage = "Diagnoza jest wymagana.")]
    public string Diagnosis { get; set; } = string.Empty;

    [Required(ErrorMessage = "Zalecenia są wymagane.")]
    public string Recommendations { get; set; } = string.Empty;
}