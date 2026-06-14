using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.MedicalRecords;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly IMedicalRecordService _service;

    public DeleteModel(IMedicalRecordService service)
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

    public async Task<IActionResult> OnPostAsync(int id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return RedirectToPage("./Index");
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
