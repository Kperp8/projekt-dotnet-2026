using Domain.Medications;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MedicationsRepository : IMedicationsRepository
{
    private readonly AppDbContext _context;

    public MedicationsRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Medication?> GetByIdAsync(int id)
    {
        return await _context.Medications
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<IReadOnlyList<Medication>> GetByNameAsync(string name)
    {
        return await _context.Medications
            .Where(m => m.Name == name)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }
    
    public async Task<IReadOnlyList<Medication>> GetByCostAsync(Decimal cost)
    {
        return await _context.Medications
            .Where(m => m.Cost == cost)
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Medication>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Medications
            .OrderBy(m => m.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Medication>> SearchAsync(string query)
    {
        return await _context.Medications
            .Where(m => m.Name.Contains(query) || m.Dosing.Contains(query))
            .OrderBy(m => m.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Medication medication)
    {
        await _context.Medications.AddAsync(medication);
    }

    public Task UpdateAsync(Medication medication)
    {
        _context.Medications.Update(medication);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Medication medication)
    {
        _context.Medications.Remove(medication);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
