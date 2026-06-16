public class Medication
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Dosing { get; set; } = null!;

    public int Quantity { get; set; }

    public Decimal Cost { get; set; }
}