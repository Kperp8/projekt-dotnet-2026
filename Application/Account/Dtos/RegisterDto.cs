using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dtos;

public class RegisterDto
{
    [Required(ErrorMessage = "Imie jest wymagane.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Imie musi mieć od 3 do 200 znaków.")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Nazwisko jest wymagane.")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Nazwisko musi mieć od 3 do 200 znaków.")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email jest wymagane.")]
    [EmailAddress(ErrorMessage = "Adres email musi być poprawny")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [StringLength(24, MinimumLength = 8, ErrorMessage = "Hasło musi mieć od 8 do 24 znaków.")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Hasło jest wymagane.")]
    [Compare(nameof(Password), ErrorMessage = "Hasło musi się zgadzać")]
    public string ComparePassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trzeba wybrać rolę")]
    public string Role { get; set; } = "Rejestratorka";
}