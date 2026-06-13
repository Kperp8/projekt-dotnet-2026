public class Visit
{
    public int Id { get; set; }

    public int PatientId {get; set;}

    public Patient Patient { get; set; } = null!;

    public VisitStatus Status { get; set; } = VisitStatus.Planned;

    public DateTime ScheduledAt { get; set; }

    public string? AssignedDoctorId { get; set; }

    public ICollection<Procedures> ProceduresPerformed {get; set;} = new List<Procedures>();

    public ICollection<ClinicalNotes> ClinicalNotes {get; set;}  = new List<ClinicalNotes>();
}
