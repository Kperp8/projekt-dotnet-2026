using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Visits;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CancelModel : PageModel
{
    private readonly IVisitService _visitService;

    public CancelModel(IVisitService visitService)
    {
        _visitService = visitService;
    }

    public VisitDetailsDto Visit { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var visit = await _visitService.GetByIdAsync(id);
        if (visit is null)
            return NotFound();

        Visit = visit;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _visitService.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            var visit = await _visitService.GetByIdAsync(id);
            if (visit is null)
                return NotFound();

            Visit = visit;
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }
}
