public class MedicalRecord
{
    public int Id { get; set; }

    public int PatientId { get; set; }

    public Patient Patient { get; set; } = null!;

    public string BloodType { get; set; } = null!;

    public string Allergies { get; set; } = null!;

    public ICollection<string> ChronicDiseases { get; set; } = new List<string>();

    public ICollection<string> Notes { get; set; } = new List<string>();

    public DateTime CreatedAt { get; set; }

    public ICollection<MedicalDocument> MedicalDocuments { get; set; } = new List<MedicalDocument>();
}