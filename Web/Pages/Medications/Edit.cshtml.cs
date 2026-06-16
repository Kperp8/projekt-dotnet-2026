using Application.Medications.Dtos;
using Application.Medications.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Medications;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IMedicationService _service;

    public EditModel(IMedicationService service)
    {
        _service = service;
    }

    [BindProperty]
    public MedicationUpdateDto Input { get; set; } = new();

    public int MedicationId { get; set; }
    public string MedicationName { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var medication = await _service.GetByIdAsync(id);
        if (medication is null)
            return NotFound();

        MedicationId = id;
        MedicationName = medication.Name;
        Input.Dosing = medication.Dosing;
        Input.Cost = medication.Cost;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            MedicationId = id;
            return Page();
        }

        try
        {
            await _service.UpdateAsync(id, Input);
            return RedirectToPage("./Details", new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
