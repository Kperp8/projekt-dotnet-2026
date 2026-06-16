using System.ComponentModel.DataAnnotations;

public class Procedure
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Nazwa jest wymagana.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nazwa musi mieć od 3 do 200 znaków.")]
    public string Name { get; set; } = null!;

    [StringLength(350, ErrorMessage = "Opis może mieć maksymalnie 350 znaków.")]
    public string Description { get; set; } = null!;

    [Range(0.01, 1000000, ErrorMessage = "Cena musi być większa niż 0.")]
    public decimal Price { get; set; }
}