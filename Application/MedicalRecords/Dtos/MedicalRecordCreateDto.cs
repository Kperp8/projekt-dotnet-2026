using System.ComponentModel.DataAnnotations;

namespace Application.MedicalRecords.Dtos;

public class MedicalRecordCreateDto
{
    [Required(ErrorMessage = "Pacjent jest wymagany")]
    public int PatientId {get; set;}

    [Required(ErrorMessage = "Typ krwi jest wymagany")]
    public string BloodType {get; set;} = string.Empty;

    [Required(ErrorMessage = "Alergie są wymagane")]
    public ICollection<string> Allergies {get; set;} = new List<string>();
    
    [Required(ErrorMessage = "Choroby przewlekłe są wymagane")]
    public ICollection<string> ChronicDiseases {get; set;} = new List<string>();
}