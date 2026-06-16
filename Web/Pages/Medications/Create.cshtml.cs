using Application.Medications.Dtos;
using Application.Medications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Medications;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IMedicationService _service;

    public CreateModel(IMedicationService service)
    {
        _service = service;
    }

    [BindProperty]
    public MedicationCreateDto Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var created = await _service.CreateAsync(Input);
            return RedirectToPage("./Details", new { id = created.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
