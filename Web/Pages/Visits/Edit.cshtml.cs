using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Visits;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IVisitService _visitService;

    public EditModel(IVisitService visitService)
    {
        _visitService = visitService;
    }

    [BindProperty]
    public VisitUpdateDto Input { get; set; } = new();

    public int VisitId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var visit = await _visitService.GetByIdAsync(id);
        if (visit is null)
            return NotFound();

        VisitId = id;
        PatientFullName = visit.PatientFullName;
        Input = new VisitUpdateDto
        {
            ScheduledAt    = visit.ScheduledAt,
            Status         = visit.Status,
            AssignedDoctorId = visit.AssignedDoctorId
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            VisitId = id;
            return Page();
        }

        try
        {
            await _visitService.UpdateAsync(id, Input);
            return RedirectToPage("./Details", new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
