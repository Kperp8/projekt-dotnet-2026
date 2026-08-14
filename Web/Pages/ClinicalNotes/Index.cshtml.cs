using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.ClinicalNotes;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IClinicalNotesService _service;
    public const int PageSize = 20;

    public IndexModel(IClinicalNotesService service)
    {
        _service = service;
    }

    public IReadOnlyList<ClinicalNotesListItemDto> Notes { get; set; } = [];
    public string? Query { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        CurrentPage = page < 1 ? 1 : page;

        Notes = string.IsNullOrWhiteSpace(query)
            ? await _service.GetPagedAsync(CurrentPage, PageSize)
            : await _service.SearchAsync(query);
    }
}
