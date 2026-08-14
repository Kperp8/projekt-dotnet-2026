using Domain.Procedures;

public class Visit
{
    public int Id { get; set; }

    public int PatientId {get; set;}

    public Patient Patient { get; set; } = null!;

    public VisitStatus Status { get; set; } = VisitStatus.Planned;

    public DateTime ScheduledAt { get; set; }

    public string? AssignedDoctorId { get; set; }

    public ICollection<Procedure> ProceduresPerformed {get; set;} = new List<Procedure>();

    public ICollection<ClinicalNote> ClinicalNotes {get; set;}  = new List<ClinicalNote>();
}
