namespace Application.Visits.Dtos;

/// <summary>Pełne DTO wizyty zwracane przez GET /visits/{id}.</summary>
public class VisitDetailsDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public string? AssignedDoctorId { get; set; }
    public IReadOnlyList<string> ProceduresPerformed { get; set; } = [];
    public IReadOnlyList<string> ClinicalNotes { get; set; } = [];
}