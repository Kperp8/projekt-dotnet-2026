using System.ComponentModel.DataAnnotations;

namespace Application.Procedure.Dtos;

public class ProceduresCreateDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    public string ProcedureName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Opis jest wymagany.")]
    public string Description { get; set; } = string.Empty;
    [Required(ErrorMessage = "Cena wizyty jest wymagana.")]
    public Decimal Price { get; set; }
}