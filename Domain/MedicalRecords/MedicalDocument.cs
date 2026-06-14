public class MedicalDocument
{
    public int Id { get; set; }

    public int MedicalRecordId {get; set;}

    public MedicalRecord MedicalRecord {get; set;} = null!;

    public string FileName {get; set;} = null!;

    public string FilePath {get; set;} = null!;

    public string DocumentType {get; set;} = null!;

    public DateTime UploadedAt {get; set;}
}