namespace Domain.Patients;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(int id);
    Task<IReadOnlyList<Patient>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Patient>> SearchAsync(string query);
    Task<bool> ExistsByPeselAsync(string pesel);
    Task<Patient?> FindByNameAsync(string firstName, string lastName);
    Task AddAsync(Patient patient);
    Task UpdateAsync(Patient patient);
    Task SaveChangesAsync();
}
