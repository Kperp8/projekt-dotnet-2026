using System.ComponentModel.DataAnnotations;

namespace Application.Procedure.Dtos;

public class ProceduresUpdateDto
{
    public string Description { get; set; } = string.Empty;
    public Decimal Price { get; set; }
}