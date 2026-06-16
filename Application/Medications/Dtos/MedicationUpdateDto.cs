using System.ComponentModel.DataAnnotations;

namespace Application.Medications.Dtos;

public class MedicationUpdateDto
{
    public string? Dosing { get; set; }
    public Decimal Cost { get; set; }
}