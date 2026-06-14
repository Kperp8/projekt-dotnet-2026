using System.ComponentModel.DataAnnotations;

namespace Application.MedicalRecords.Dtos;

public class MedicalRecordUpdateDto
{
    public ICollection<string>? Allergies { get; set; }
    public ICollection<string>? ChronicDiseases { get; set; }
    public ICollection<string>? Notes { get; set; }
}