using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Visits;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IVisitService _visitService;

    public DetailsModel(IVisitService visitService)
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

    public static string StatusLabel(VisitStatus status) => status switch
    {
        VisitStatus.Planned    => "Zaplanowana",
        VisitStatus.InProgress => "W trakcie",
        VisitStatus.Completed  => "Zakończona",
        VisitStatus.Cancelled  => "Anulowana",
        _                      => status.ToString()
    };

    public static string StatusBadgeClass(VisitStatus status) => status switch
    {
        VisitStatus.Planned    => "bg-primary",
        VisitStatus.InProgress => "bg-warning text-dark",
        VisitStatus.Completed  => "bg-success",
        VisitStatus.Cancelled  => "bg-secondary",
        _                      => "bg-light text-dark"
    };
}
