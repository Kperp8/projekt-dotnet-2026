namespace Application.Patients.Dtos;

// TODO: rozbudować gdy moduł Wizyt zostanie zaimplementowany
public class VisitListItemDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; } = string.Empty;
    public string DoctorFullName { get; set; } = string.Empty;
}
