namespace Application.MedicalRecords.Dtos;

/// <summary>Lekkie DTO używane na liście rekordów.</summary>
public class MedicalRecordListItemDto
{
    public int Id {get; set;}
    public int PatientId {get; set;}
    public string PatientFullName {get; set;} = string.Empty;
    public string BloodType {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;}
}