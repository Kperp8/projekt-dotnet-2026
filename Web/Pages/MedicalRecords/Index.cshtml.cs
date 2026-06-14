using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.MedicalRecords;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IMedicalRecordService _service;
    public const int PageSize = 20;

    public IndexModel(IMedicalRecordService service)
    {
        _service = service;
    }

    public IReadOnlyList<MedicalRecordListItemDto> Records { get; set; } = [];
    public string? Query { get; set; }
    public int CurrentPage { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        CurrentPage = page < 1 ? 1 : page;

        Records = string.IsNullOrWhiteSpace(query)
            ? await _service.GetPagedAsync(CurrentPage, PageSize)
            : await _service.SearchAsync(query);
    }
}
