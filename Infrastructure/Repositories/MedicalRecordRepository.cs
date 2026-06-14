using Domain.MedicalRecords;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MedicalRecordRepository : IMedicalRecordsRepository
{
    private readonly AppDbContext _context;

    public MedicalRecordRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<MedicalRecord?> GetByIdAsync(int id)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.MedicalDocuments)
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IReadOnlyList<MedicalRecord>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicalRecord>> SearchAsync(string query)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Where(m =>
                m.Patient.LastName.Contains(query) ||
                m.Patient.Pesel.Contains(query))
            .OrderBy(m => m.Patient.LastName)
            .ThenBy(m => m.Patient.FirstName)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicalRecord>> GetByPatientIdAsync(int patientId)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Include(m => m.MedicalDocuments)
            .Where(m => m.PatientId == patientId)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicalRecord>> GetByBloodTypeAsync(string bloodType)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Where(m => m.BloodType == bloodType)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicalRecord>> GetByAllergiesAsync(string allergy)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Where(m => m.Allergies.Contains(allergy))
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<MedicalRecord>> GetByCreationDateAsync(DateOnly date)
    {
        return await _context.MedicalRecords
            .Include(m => m.Patient)
            .Where(m => DateOnly.FromDateTime(m.CreatedAt) == date)
            .OrderByDescending(m => m.CreatedAt)
            .ToListAsync();
    }

    public async Task AddAsync(MedicalRecord record)
    {
        await _context.MedicalRecords.AddAsync(record);
    }

    public async Task AddDocumentAsync(MedicalDocument document)
    {
        await _context.MedicalDocuments.AddAsync(document);
    }

    public Task UpdateAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Update(record);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(MedicalRecord record)
    {
        _context.MedicalRecords.Remove(record);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}