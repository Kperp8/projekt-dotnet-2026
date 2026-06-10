using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Patients;

[Authorize(Roles = "Admin,Rejestratorka")]
public class DeleteModel : PageModel
{
    private readonly IPatientService _patientService;

    public DeleteModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public PatientDetailsDto Patient { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient is null)
            return NotFound();

        Patient = patient;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _patientService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
