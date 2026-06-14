using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Web.Pages.MedicalRecords;

[Authorize(Roles = "Admin,Rejestratorka")]
public class EditModel : PageModel
{
    private readonly IMedicalRecordService _service;

    public EditModel(IMedicalRecordService service)
    {
        _service = service;
    }

    [BindProperty]
    public MedicalRecordUpdateDto Input { get; set; } = new();

    [BindProperty]
    public string AllergiesText { get; set; } = string.Empty;

    [BindProperty]
    public string ChronicDiseasesText { get; set; } = string.Empty;

    [BindProperty]
    public string NotesText { get; set; } = string.Empty;

    public int RecordId { get; set; }
    public string PatientFullName { get; set; } = string.Empty;
    public string BloodType { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        var record = await _service.GetByIdAsync(id);
        if (record is null)
            return NotFound();

        RecordId = id;
        PatientFullName = record.PatientFullName;
        BloodType = record.BloodType;
        AllergiesText = string.Join('\n', record.Allergies);
        ChronicDiseasesText = string.Join('\n', record.ChronicDiseases);
        NotesText = string.Join('\n', record.Notes);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(int id)
    {
        Input.Allergies = ParseLines(AllergiesText);
        Input.ChronicDiseases = ParseLines(ChronicDiseasesText);
        Input.Notes = ParseLines(NotesText);

        if (!ModelState.IsValid)
        {
            RecordId = id;
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

    private static List<string> ParseLines(string text) =>
        text.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(s => s.Trim())
            .Where(s => s.Length > 0)
            .ToList();
}
