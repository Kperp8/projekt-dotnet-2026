namespace Domain.Visits;

public interface IVisitsRepository
{
    Task<Visit?> GetByIdAsync(int id);
    Task<IReadOnlyList<Visit>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Visit>> SearchAsync(string query);
    Task<IReadOnlyList<Visit>> GetByPatientIdAsync(int patientId);
    Task<IReadOnlyList<Visit>> GetByDoctorIdAsync(string doctorId);
    Task<IReadOnlyList<Visit>> GetByStatusAsync(VisitStatus status);
    Task<IReadOnlyList<Visit>> GetScheduledForDateAsync(DateOnly date);
    Task AddAsync(Visit visit);
    Task UpdateAsync(Visit visit);
    Task SaveChangesAsync();
}