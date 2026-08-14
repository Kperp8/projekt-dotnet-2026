using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.ClinicalNotes;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IClinicalNotesService _service;

    public DetailsModel(IClinicalNotesService service)
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
}
