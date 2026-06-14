using System.ComponentModel.DataAnnotations;

public class Patient
{
    public int Id { get; set; }

    [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć dokładnie 11 znaków.")]
    public string Pesel { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;
    
    public DateTime BirthDate { get; set; }
    
    public string InsuranceNumber { get; set; } = string.Empty;
    
    public string PhoneNumber { get; set; } = string.Empty;
    
    public string Email { get; set; } = string.Empty;
    
    public string Address { get; set; } = string.Empty;
    
    public bool IsDeleted { get; set; }
    
    public ICollection<Visit> Visits { get; set; } = new List<Visit>(); // TODO: przenieść wizyty pacjenta do kartoteki (karty pacjentów?)
}