namespace Application.Procedures.Dtos;

/// <summary>Pełne DTO procedury zwracane przez GET /procedures/{id}.</summary>
public class ProceduresDetailsDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
}