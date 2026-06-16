using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Procedures;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IProceduresService _proceduresService;

    public CreateModel(IProceduresService proceduresService)
    {
        _proceduresService = proceduresService;
    }

    [BindProperty]
    public ProceduresCreateDto Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var created = await _proceduresService.CreateAsync(Input);
        return RedirectToPage("./Details", new { id = created.Id });
    }
}
