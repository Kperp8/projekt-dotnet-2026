using Microsoft.AspNetCore.Http;

namespace Application.MedicalRecords.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(IFormFile file);
}