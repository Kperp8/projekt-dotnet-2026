namespace Application.Medications.Dtos;

/// <summary>Pełne DTO leku zwracane przez GET /medications/{id}.</summary>
public class MedicationsDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Dosing { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Decimal Cost { get; set; }
}