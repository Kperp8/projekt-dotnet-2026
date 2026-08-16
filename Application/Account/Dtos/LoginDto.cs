using System.ComponentModel.DataAnnotations;

namespace Application.Account.Dtos;

public class LoginDto
{
    [Required(ErrorMessage = "Trzeba podać email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Trzeba podać hasło")]
    [StringLength(24, MinimumLength = 8, ErrorMessage = "Hasło musi mieć od 8 do 24 znaków.")]
    public string Password { get; set; } = string.Empty;

    public bool RememberMe { get; set; } = false;
}