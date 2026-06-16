namespace Domain.Procedures;

public interface IProceduresRepository
{
    Task<Procedure?> GetByIdAsync(int id);
    Task<IReadOnlyList<Procedure>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Procedure>> SearchAsync(string query);
    Task<IReadOnlyList<Procedure>> GetByMaxPriceAsync(decimal maxPrice);
    Task AddAsync(Procedure procedure);
    Task UpdateAsync(Procedure procedure);
    Task DeleteAsync(Procedure procedure);
    Task SaveChangesAsync();
}