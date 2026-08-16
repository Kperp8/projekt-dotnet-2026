using Application.Account.Dtos;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Account;

[AllowAnonymous]
public class RegisterModel(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : PageModel
{
    [BindProperty]
    public RegisterDto Input { get; set; } = new();

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var user = new AppUser
        {
            UserName = Input.Email,
            Email = Input.Email,
            FirstName = Input.FirstName,
            LastName = Input.LastName
        };

        var result = await userManager.CreateAsync(user, Input.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return Page();
        }

        await userManager.AddToRoleAsync(user, Input.Role);
        await signInManager.SignInAsync(user, isPersistent: false);

        return RedirectToPage("/Index");
    }
}
