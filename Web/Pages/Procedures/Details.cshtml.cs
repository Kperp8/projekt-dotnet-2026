using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.Procedures;

[Authorize]
public class DetailsModel : PageModel
{
    private readonly IProceduresService _proceduresService;

    public DetailsModel(IProceduresService proceduresService)
    {
        _proceduresService = proceduresService;
    }

    public ProceduresDetailsDto Procedure { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var procedure = await _proceduresService.GetByIdAsync(id);
        if (procedure is null)
            return NotFound();

        Procedure = procedure;
        return Page();
    }
}
