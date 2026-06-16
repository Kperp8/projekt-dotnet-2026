using System.ComponentModel.DataAnnotations;

namespace Application.Procedures.Dtos;

public class ProceduresCreateDto
{
    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć od 3 do 200 znaków.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Opis jest wymagany.")]
    [StringLength(350, ErrorMessage = "Opis może mieć maksymalnie 350 znaków.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Cena jest wymagana.")]
    [Range(0.01, 1000000, ErrorMessage = "Cena musi być większa niż 0.")]
    public decimal Price { get; set; }
}