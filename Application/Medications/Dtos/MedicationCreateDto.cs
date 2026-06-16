using System.ComponentModel.DataAnnotations;

namespace Application.Medications.Dtos;

public class MedicationCreateDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Dozowanie jest wymagane")]
    public string Dosing { get; set; } = string.Empty;

    [Required(ErrorMessage = "Koszt jest wymagany")]
    public Decimal Cost { get; set; }
}