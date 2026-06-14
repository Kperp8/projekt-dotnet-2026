using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.MedicalRecords;

[Authorize(Roles = "Admin,Rejestratorka")]
public class CreateModel : PageModel
{
    private readonly IMedicalRecordService _service;

    public CreateModel(IMedicalRecordService service)
    {
        _service = service;
    }

    [BindProperty]
    public MedicalRecordCreateDto Input { get; set; } = new();

    [BindProperty]
    public string AllergiesText { get; set; } = string.Empty;

    [BindProperty]
    public string ChronicDiseasesText { get; set; } = string.Empty;

    public void OnGet(int? patientId)
    {
        if (patientId.HasValue)
            Input.PatientId = patientId.Value;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Input.Allergies = ParseLines(AllergiesText);
        Input.ChronicDiseases = ParseLines(ChronicDiseasesText);

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var created = await _service.CreateAsync(Input);
            return RedirectToPage("./Details", new { id = created.Id });
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return Page();
        }
    }

    private static List<string> ParseLines(string text) =>
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
}
