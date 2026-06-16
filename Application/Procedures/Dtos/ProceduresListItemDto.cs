namespace Application.Procedures.Dtos;

/// <summary>Lekkie DTO używane na liście procedur.</summary>
public class ProceduresListItemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}