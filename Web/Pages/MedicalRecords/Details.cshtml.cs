using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.MedicalRecords;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IMedicalRecordService _service;

    public DetailsModel(IMedicalRecordService service)
    {
        _service = service;
    }

    public MedicalRecordDetailsDto Record { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var record = await _service.GetByIdAsync(id);
        if (record is null)
            return NotFound();

        Record = record;
        return Page();
    }
}
