using Domain.ClinicalNotes;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ClinicalNotesRepository : IClinicalNotesRepository
{
    private readonly AppDbContext _context;

    public ClinicalNotesRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ClinicalNote?> GetByIdAsync(int id)
    {
        return await _context.ClinicalNotes
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IReadOnlyList<ClinicalNote>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.ClinicalNotes
            .OrderBy(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<ClinicalNote>> SearchAsync(string query)
    {
        return await _context.ClinicalNotes
            .Where(c => c.MedicalHistory.Contains(query) || c.Diagnosis.Contains(query) || c.Recommendations.Contains(query))
            .OrderBy(c => c.Id)
            .ToListAsync();
    }

    public async Task AddAsync(ClinicalNote note)
    {
        await _context.ClinicalNotes.AddAsync(note);
    }

    public Task UpdateAsync(ClinicalNote note)
    {
        _context.ClinicalNotes.Update(note);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(ClinicalNote note)
    {
        _context.ClinicalNotes.Remove(note);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}