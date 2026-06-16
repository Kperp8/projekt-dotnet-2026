using System.ComponentModel.DataAnnotations;

namespace Application.Procedures.Dtos;

public class ProceduresUpdateDto
{
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć od 3 do 200 znaków.")]
    public string? Name { get; set; }

    [StringLength(350, ErrorMessage = "Opis może mieć maksymalnie 350 znaków.")]
    public string? Description { get; set; }

    [Range(0.01, 1000000, ErrorMessage = "Cena musi być większa niż 0.")]
    public decimal? Price { get; set; }
}