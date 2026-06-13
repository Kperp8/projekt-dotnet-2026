using System.ComponentModel.DataAnnotations;

namespace Application.Patients.Dtos;

public class PatientCreateDto
{
    [Required(ErrorMessage = "PESEL jest wymagany.")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "PESEL musi mieć dokładnie 11 znaków.")]
    [RegularExpression(@"^\d{11}$", ErrorMessage = "PESEL może zawierać tylko cyfry.")]
    public string Pesel { get; set; } = string.Empty;

    [Required(ErrorMessage = "Imię jest wymagane.")]
    [StringLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [StringLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Data urodzenia jest wymagana.")]
    public DateTime BirthDate { get; set; }

    [StringLength(20)]
    public string InsuranceNumber { get; set; } = string.Empty;

    [Phone(ErrorMessage = "Nieprawidłowy format numeru telefonu.")]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Nieprawidłowy format adresu e-mail.")]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(300)]
    public string Address { get; set; } = string.Empty;
}
