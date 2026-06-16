using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Procedures;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IProceduresService _proceduresService;
    public const int PageSize = 20;

    public IndexModel(IProceduresService proceduresService)
    {
        _proceduresService = proceduresService;
    }

    public IReadOnlyList<ProceduresListItemDto> Procedures { get; set; } = [];
    public string? Query { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        CurrentPage = page < 1 ? 1 : page;

        Procedures = string.IsNullOrWhiteSpace(query)
            ? await _proceduresService.GetPagedAsync(CurrentPage, PageSize)
            : await _proceduresService.SearchAsync(query);
    }
}
