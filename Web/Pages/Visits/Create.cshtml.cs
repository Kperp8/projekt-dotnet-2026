using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Visits;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IVisitService _visitService;
    private readonly IPatientService _patientService;

    public CreateModel(IVisitService visitService, IPatientService patientService)
    {
        _visitService = visitService;
        _patientService = patientService;
    }

    [BindProperty]
    public VisitCreateDto Input { get; set; } = new();

    public IReadOnlyList<PatientListItemDto> Patients { get; set; } = [];

    public async Task OnGetAsync()
    {
        Patients = await _patientService.GetPagedAsync(1, 1000);
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid)
        {
            Patients = await _patientService.GetPagedAsync(1, 1000);
            return Page();
        }

        var created = await _visitService.CreateAsync(Input);
        return RedirectToPage("./Details", new { id = created.Id });
    }
}
