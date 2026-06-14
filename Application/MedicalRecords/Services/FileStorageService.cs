using Application.MedicalRecords.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;

namespace Application.MedicalRecords.Services;

public class FileStorageService : IFileStorageService
{
    private readonly IWebHostEnvironment _environment;

    public FileStorageService(IWebHostEnvironment environment)
    {
        _environment = environment;
    }

    public async Task<string> SaveFileAsync(IFormFile file)
    {
        var uploadsPath = Path.Combine(
            _environment.WebRootPath,
            "uploads");

        Directory.CreateDirectory(uploadsPath);

        var fileName =
            $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";

        var fullPath =
            Path.Combine(uploadsPath, fileName);

        using var stream = new FileStream(
            fullPath,
            FileMode.Create);

        await file.CopyToAsync(stream);

        return $"/uploads/{fileName}";
    }
}