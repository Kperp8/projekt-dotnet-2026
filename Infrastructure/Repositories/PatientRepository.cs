using Domain.Patients;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly AppDbContext _context;

    public PatientRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(int id)
    {
        return await _context.Patients
            .Include(p => p.Visits)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IReadOnlyList<Patient>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Patients
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Patient>> SearchAsync(string query)
    {
        // jeśli query ma dokładnie 11 cyfr – szukamy po PESEL (korzysta z indeksu IX_Patients_Pesel)
        // w przeciwnym razie – szukamy po fragmencie nazwiska (korzysta z indeksu IX_Patients_LastName)
        bool isPesel = query.Length == 11 && query.All(char.IsDigit);

        if (isPesel)
        {
            return await _context.Patients
                .Where(p => p.Pesel == query)
                .ToListAsync();
        }

        return await _context.Patients
            .Where(p => p.LastName.Contains(query))
            .OrderBy(p => p.LastName)
            .ThenBy(p => p.FirstName)
            .ToListAsync();
    }

    public async Task<bool> ExistsByPeselAsync(string pesel)
    {
        // IgnoreQueryFilters – sprawdzamy duplikat PESEL nawet wśród usuniętych pacjentów
        return await _context.Patients
            .IgnoreQueryFilters()
            .AnyAsync(p => p.Pesel == pesel);
    }

    public async Task<Patient?> FindByNameAsync(string firstName, string lastName)
    {
        return await _context.Patients
            .Include(p => p.Visits)
            .FirstOrDefaultAsync(p =>
                p.FirstName == firstName &&
                p.LastName == lastName);
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
    }

    public Task UpdateAsync(Patient patient)
    {
        _context.Patients.Update(patient);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
