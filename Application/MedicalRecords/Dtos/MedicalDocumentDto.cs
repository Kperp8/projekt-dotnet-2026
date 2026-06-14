namespace Application.MedicalRecords.Dtos;

public class MedicalDocumentDto
{
    public int Id { get; set; }
    public string FileName { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public DateTime UploadedAt { get; set; }
}
