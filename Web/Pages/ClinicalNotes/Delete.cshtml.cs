using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.ClinicalNotes;

[Authorize(Roles = "Admin")]
public class DeleteModel : PageModel
{
    private readonly IClinicalNotesService _service;

    public DeleteModel(IClinicalNotesService service)
    {
        _service = service;
    }

    public ClinicalNotesDetailsDto Note { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var note = await _service.GetByIdAsync(id);
        if (note is null)
            return NotFound();

        Note = note;
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
