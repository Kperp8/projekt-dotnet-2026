using System.ComponentModel.DataAnnotations;

public class Procedures
{
    public int Id { get; set; }

    [StringLength(350, ErrorMessage = "Opis może mieć maksymalnie 350 znaków")]
    public string Description { get; set; } = null!;

    public Decimal Price { get; set; }
}