namespace Domain.Procedure;

public interface IProceduresRepository
{
    Task<Procedures?> GetByIdAsync(int id);
    Task<IReadOnlyList<Procedures>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<Procedures>> SearchAsync(string query);
    Task<IReadOnlyList<Procedures>> GetByName(string procedureName);
    Task<IReadOnlyList<Procedures>> GetByPrice(Decimal price);
    Task AddAsync(Procedures procedure);
    Task UpdateAsync(Procedures procedure);
    Task DeleteAsync(Procedures procedure);
    Task SaveChangesAsync();
}