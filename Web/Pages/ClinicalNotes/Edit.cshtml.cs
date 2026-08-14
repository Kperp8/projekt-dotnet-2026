using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.ClinicalNotes;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IClinicalNotesService _service;

    public EditModel(IClinicalNotesService service)
    {
        _service = service;
    }

    [BindProperty]
    public ClinicalNotesUpdateDto Input { get; set; } = new();

    public int NoteId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var note = await _service.GetByIdAsync(id);
        if (note is null)
            return NotFound();

        NoteId = id;
        Input = new ClinicalNotesUpdateDto
        {
            MedicalHistory  = note.MedicalHistory,
            Diagnosis       = note.Diagnosis,
            Recommendations = note.Recommendations
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            NoteId = id;
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
