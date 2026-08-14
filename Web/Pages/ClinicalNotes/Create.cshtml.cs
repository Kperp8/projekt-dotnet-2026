using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.ClinicalNotes;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IClinicalNotesService _service;

    public CreateModel(IClinicalNotesService service)
    {
        _service = service;
    }

    [BindProperty]
    public ClinicalNotesCreateDto Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        var created = await _service.CreateAsync(Input);
        return RedirectToPage("./Details", new { id = created.Id });
    }
}
