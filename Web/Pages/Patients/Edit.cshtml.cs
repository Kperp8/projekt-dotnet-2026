using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Patients;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IPatientService _patientService;

    public EditModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [BindProperty]
    public PatientUpdateDto Input { get; set; } = new();

    public int PatientId { get; set; }
    public string Pesel { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient is null)
            return NotFound();

        PatientId = id;
        Pesel = patient.Pesel;
        Input = new PatientUpdateDto
        {
            FirstName = patient.FirstName,
            LastName = patient.LastName,
            BirthDate = patient.BirthDate,
            InsuranceNumber = patient.InsuranceNumber,
            PhoneNumber = patient.PhoneNumber,
            Email = patient.Email,
            Address = patient.Address
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            PatientId = id;
            return Page();
        }

        try
        {
            await _patientService.UpdateAsync(id, Input);
            return RedirectToPage("./Details", new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
