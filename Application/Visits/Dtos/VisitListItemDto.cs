namespace Application.Visits.Dtos;

/// Lekkie DTO używane na liście wizyt.
public class VisitListItemDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public string? AssignedDoctorId { get; set; }
}