using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Patients;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IPatientService _patientService;

    public CreateModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [BindProperty]
    public PatientCreateDto Input { get; set; } = new();

    public void OnGet() { }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
            return Page();

        try
        {
            var created = await _patientService.CreateAsync(Input);
            return RedirectToPage("./Details", new { id = created.Id });
        }
        catch (InvalidOperationException ex)
        {
            // duplikat PESEL
            ModelState.AddModelError(nameof(Input.Pesel), ex.Message);
            return Page();
        }
    }
}
