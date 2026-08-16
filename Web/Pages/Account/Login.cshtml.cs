using Application.Account.Dtos;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Account;

[AllowAnonymous]
public class LoginModel(SignInManager<AppUser> signInManager) : PageModel
{
    [BindProperty]
    public LoginDto Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var result = await signInManager.PasswordSignInAsync(
            Input.Email, Input.Password, Input.RememberMe, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            ModelState.AddModelError(string.Empty, "Nieprawidłowy email lub hasło.");
            return Page();
        }

        return RedirectToPage("/Index");
    }
}
