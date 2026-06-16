using Domain.Procedures;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ProceduresRepository : IProceduresRepository
{
    private readonly AppDbContext _context;

    public ProceduresRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Procedure?> GetByIdAsync(int id)
    {
        return await _context.Procedures
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Procedure>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Procedures
            .OrderBy(p => p.Name)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Procedure>> SearchAsync(string query)
    {
        return await _context.Procedures
            .Where(p => p.Name.Contains(query) || p.Description.Contains(query))
            .OrderBy(p => p.Name)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Procedure>> GetByMaxPriceAsync(decimal maxPrice)
    {
        return await _context.Procedures
            .Where(p => p.Price <= maxPrice)
            .OrderBy(p => p.Price)
            .ToListAsync();
    }

    public async Task AddAsync(Procedure procedure)
    {
        await _context.Procedures.AddAsync(procedure);
    }

    public Task UpdateAsync(Procedure procedure)
    {
        _context.Procedures.Update(procedure);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Procedure procedure)
    {
        _context.Procedures.Remove(procedure);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
