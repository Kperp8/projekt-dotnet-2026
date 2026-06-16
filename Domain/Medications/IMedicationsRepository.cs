namespace Domain.Medications;

public interface IMedicationsRepository
{
    Task<Medication?> GetByIdAsync(int id);
    Task<IReadOnlyList<Medication>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Medication>> SearchAsync(string query);
    Task<IReadOnlyList<Medication>> GetByNameAsync(string name);
    Task<IReadOnlyList<Medication>> GetByCostAsync(Decimal cost);
    Task AddAsync(Medication medication);
    Task UpdateAsync(Medication medication);
    Task DeleteAsync(Medication medication);
    Task SaveChangesAsync();
}