using Application.Medications.Dtos;
using Application.Medications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Medications;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly IMedicationService _service;

    public DeleteModel(IMedicationService service)
    {
        _service = service;
    }

    public MedicationDetailsDto Medication { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var medication = await _service.GetByIdAsync(id);
        if (medication is null)
            return NotFound();

        Medication = medication;
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
