public class ClinicalNote
{
    public int Id {get; set;}

    public string MedicalHistory {get; set;} = string.Empty;
    
    public string Diagnosis {get; set;} = string.Empty;
    
    public string Recommendations {get; set;} = string.Empty;
}