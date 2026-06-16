namespace Application.Medications.Dtos;

/// <summary>Lekkie DTO używane na liście leków.</summary>
public class MedicationsListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public Decimal Cost { get; set; }
}