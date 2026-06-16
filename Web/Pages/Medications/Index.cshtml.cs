using Application.Medications.Dtos;
using Application.Medications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Medications;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMedicationService _service;
    public const int PageSize = 20;

    public IndexModel(IMedicationService service)
    {
        _service = service;
    }

    public IReadOnlyList<MedicationListItemDto> Medications { get; set; } = [];
    public string? Query { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        CurrentPage = page < 1 ? 1 : page;

        Medications = string.IsNullOrWhiteSpace(query)
            ? await _service.GetPagedAsync(CurrentPage, PageSize)
            : await _service.SearchAsync(query);
    }
}
