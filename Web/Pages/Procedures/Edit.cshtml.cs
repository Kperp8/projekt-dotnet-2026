using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Procedures;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IProceduresService _proceduresService;

    public EditModel(IProceduresService proceduresService)
    {
        _proceduresService = proceduresService;
    }

    [BindProperty]
    public ProceduresUpdateDto Input { get; set; } = new();

    public int ProcedureId { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var procedure = await _proceduresService.GetByIdAsync(id);
        if (procedure is null)
            return NotFound();

        ProcedureId = id;
        Input = new ProceduresUpdateDto
        {
            Name        = procedure.Name,
            Description = procedure.Description,
            Price       = procedure.Price
        };

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        if (!ModelState.IsValid)
        {
            ProcedureId = id;
            return Page();
        }

        try
        {
            await _proceduresService.UpdateAsync(id, Input);
            return RedirectToPage("./Details", new { id });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}
