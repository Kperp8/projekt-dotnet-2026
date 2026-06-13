namespace Domain.Visits;

public interface IVisitsRepository
{
    Task<Visit?> GetByIdAsync(int id);
    Task<IReadOnlyList<Visit>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Visit>> SearchAsync(string query);
    Task GetByPatientIdAsync(int patientId);
    Task GetByDoctorIdAsync(string doctorId);
    Task GetByStatusAsync(VisitStatus status);
    Task GetScheduledForDateAsync(DateOnly date);
    Task AddAsync(Visit visit);
    Task UpdateAsync(Visit visit);
    Task SaveChangesAsync();
}