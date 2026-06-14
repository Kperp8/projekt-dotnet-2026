using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Web.Pages.MedicalRecords;

[Authorize]
public class UploadDocumentModel : PageModel
{
    private static readonly string[] AllowedExtensions =
        [".pdf", ".jpg", ".jpeg", ".png", ".tif", ".tiff"];

    private const long MaxFileSizeBytes = 10 * 1024 * 1024; // 10 MB

    private readonly IMedicalRecordService _service;

    public UploadDocumentModel(IMedicalRecordService service)
    {
        _service = service;
    }

    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel
    {
        public int MedicalRecordId { get; set; }

        [Required(ErrorMessage = "Wybierz typ dokumentu.")]
        [Display(Name = "Typ dokumentu")]
        public string DocumentType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wybierz plik do przesłania.")]
        [Display(Name = "Plik")]
        public IFormFile? File { get; set; }
    }

    public IActionResult OnGet(int medicalRecordId)
    {
        Input.MedicalRecordId = medicalRecordId;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Input.File is not null)
        {
            var ext = Path.GetExtension(Input.File.FileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(ext))
                ModelState.AddModelError(
                    nameof(Input.File),
                    "Dozwolone formaty: PDF, JPG, PNG, TIFF.");

            if (Input.File.Length > MaxFileSizeBytes)
                ModelState.AddModelError(
                    nameof(Input.File),
                    "Plik nie może przekraczać 10 MB.");
        }

        if (!ModelState.IsValid)
            return Page();

        try
        {
            var dto = new UploadMedicalDocumentDto
            {
                MedicalRecordId = Input.MedicalRecordId,
                DocumentType = Input.DocumentType,
                File = Input.File!
            };

            await _service.UploadDocumentAsync(dto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

        return RedirectToPage("./Details", new { id = Input.MedicalRecordId });
    }
}
