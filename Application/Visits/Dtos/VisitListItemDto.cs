namespace Application.Visits.Dtos;

/// <summary>Lekkie DTO używane na liście wizyt.</summary>
public class VisitListItemDto
{
    public int Id { get; set; }
    public int PatientId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public DateTime ScheduledAt { get; set; }
    public VisitStatus Status { get; set; }
    public string? AssignedDoctorId { get; set; }
}