using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Application.MedicalRecords.Dtos;

public class UploadMedicalDocumentDto
{
    public int MedicalRecordId { get; set; }

    public string DocumentType { get; set; } = string.Empty;

    public IFormFile File { get; set; } = null!;
}