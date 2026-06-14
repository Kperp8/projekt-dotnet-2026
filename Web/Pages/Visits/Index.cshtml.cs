using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Visits;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IVisitService _visitService;
    public const int PageSize = 20;

    public IndexModel(IVisitService visitService)
    {
        _visitService = visitService;
    }

    public IReadOnlyList<VisitListItemDto> Visits { get; set; } = [];
    public string? Query { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        CurrentPage = page < 1 ? 1 : page;

        Visits = string.IsNullOrWhiteSpace(query)
            ? await _visitService.GetPagedAsync(CurrentPage, PageSize)
            : await _visitService.SearchAsync(query);
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
