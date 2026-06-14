using Domain.Visits;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class VisitRepository : IVisitsRepository
{
    private readonly AppDbContext _context;

    public VisitRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Visit?> GetByIdAsync(int id)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .Include(v => v.ProceduresPerformed)
            .Include(v => v.ClinicalNotes)
            .FirstOrDefaultAsync(v => v.Id == id);
    }

    public async Task<IReadOnlyList<Visit>> GetPagedAsync(int page, int pageSize)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .OrderByDescending(v => v.ScheduledAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Visit>> SearchAsync(string query)
    {
        // jeśli query ma dokładnie 11 cyfr – szukamy po PESEL pacjenta
        // w przeciwnym razie – szukamy po fragmencie nazwiska pacjenta
        bool isPesel = query.Length == 11 && query.All(char.IsDigit);

        if (isPesel)
        {
            return await _context.Visits
                .Include(v => v.Patient)
                .Where(v => v.Patient.Pesel == query)
                .OrderByDescending(v => v.ScheduledAt)
                .ToListAsync();
        }

        return await _context.Visits
            .Include(v => v.Patient)
            .Where(v => v.Patient.LastName.Contains(query))
            .OrderByDescending(v => v.ScheduledAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Visit>> GetByPatientIdAsync(int patientId)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .Where(v => v.PatientId == patientId)
            .OrderByDescending(v => v.ScheduledAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Visit>> GetByDoctorIdAsync(string doctorId)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .Where(v => v.AssignedDoctorId == doctorId)
            .OrderByDescending(v => v.ScheduledAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Visit>> GetByStatusAsync(VisitStatus status)
    {
        return await _context.Visits
            .Include(v => v.Patient)
            .Where(v => v.Status == status)
            .OrderByDescending(v => v.ScheduledAt)
            .ToListAsync();
    }

    public async Task<IReadOnlyList<Visit>> GetScheduledForDateAsync(DateOnly date)
    {
        var from = date.ToDateTime(TimeOnly.MinValue);
        var to = from.AddDays(1);

        return await _context.Visits
            .Include(v => v.Patient)
            .Where(v => v.ScheduledAt >= from && v.ScheduledAt < to)
            .OrderBy(v => v.ScheduledAt)
            .ToListAsync();
    }

    public async Task AddAsync(Visit visit)
    {
        await _context.Visits.AddAsync(visit);
    }

    public Task UpdateAsync(Visit visit)
    {
        _context.Visits.Update(visit);
        return Task.CompletedTask;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
