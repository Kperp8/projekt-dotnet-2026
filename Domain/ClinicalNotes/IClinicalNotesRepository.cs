namespace Domain.ClinicalNotes;

public interface IClinicalNotesRepository
{
    Task<ClinicalNote?> GetByIdAsync(int id);
    Task<IReadOnlyList<ClinicalNote>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<ClinicalNote>> SearchAsync(string query);
    Task AddAsync(ClinicalNote note);
    Task UpdateAsync(ClinicalNote note);
    Task DeleteAsync(ClinicalNote note);
    Task SaveChangesAsync();
}