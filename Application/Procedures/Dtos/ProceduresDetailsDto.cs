namespace Application.Procedures.Dtos;

/// <summary>Pełne DTO wizyty zwracane przez GET /procedures/{id}.</summary>
public class ProceduresDetailsDto
{
    public int Id { get; set; }
    public string ProcedureName { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Decimal Price { get; set; }
}