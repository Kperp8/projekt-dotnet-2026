using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Patients;

[Authorize]
public class IndexModel : PageModel
{
    private readonly IPatientService _patientService;
    public const int PageSize = 20;

    public IndexModel(IPatientService patientService)
    {
        _patientService = patientService;
    }

    public IReadOnlyList<PatientListItemDto> Patients { get; set; } = [];
    public string? Query { get; set; }
    public int Page { get; set; } = 1;

    public async Task OnGetAsync(string? query, int page = 1)
    {
        Query = query;
        Page = page < 1 ? 1 : page;

        Patients = string.IsNullOrWhiteSpace(query)
            ? await _patientService.GetPagedAsync(Page, PageSize)
            : await _patientService.SearchAsync(query);
    }
}
