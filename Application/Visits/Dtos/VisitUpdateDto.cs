using System.ComponentModel.DataAnnotations;

namespace Application.Visits.Dtos;

public class VisitUpdateDto
{
    [Required(ErrorMessage = "Data wizyty jest wymagana.")]
    public DateTime ScheduledAt { get; set; }

    [Required(ErrorMessage = "Status wizyty jest wymagany.")]
    public VisitStatus Status { get; set; }

    public string? AssignedDoctorId { get; set; }
}