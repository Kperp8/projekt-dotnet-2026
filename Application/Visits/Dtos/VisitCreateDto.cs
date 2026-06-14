using System.ComponentModel.DataAnnotations;

namespace Application.Visits.Dtos;

public class VisitCreateDto
{
    [Required(ErrorMessage = "Pacjent jest wymagany.")]
    public int PatientId { get; set; }

    [Required(ErrorMessage = "Data wizyty jest wymagana.")]
    public DateTime ScheduledAt { get; set; }

    public string? AssignedDoctorId { get; set; }

    public VisitStatus Status { get; set; } = VisitStatus.Planned;
}