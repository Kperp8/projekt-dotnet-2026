namespace Application.Procedure.Dtos;

/// <summary>Lekkie DTO używane na liście procedur.</summary>
public class ProceduresListItemDto
{
    public int Id { get; set; }
    public string ProcedureName { get; set; } = string.Empty;
    public Decimal Price { get; set; }
}