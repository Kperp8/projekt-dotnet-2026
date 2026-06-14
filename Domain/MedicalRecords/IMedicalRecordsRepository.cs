namespace Domain.MedicalRecords;

public interface IMedicalRecordsRepository
{
    Task<MedicalRecord?> GetByIdAsync(int id);
    Task<IReadOnlyList<MedicalRecord>> GetPagedAsync(int page, int pageSize);
    Task<IReadOnlyList<MedicalRecord>> SearchAsync(string query);
    Task<IReadOnlyList<MedicalRecord>> GetByPatientIdAsync(int patientId);
    Task<IReadOnlyList<MedicalRecord>> GetByBloodTypeAsync(string bloodType);
    Task<IReadOnlyList<MedicalRecord>> GetByAllergiesAsync(string allergy);
    Task<IReadOnlyList<MedicalRecord>> GetByCreationDateAsync(DateOnly date);
    Task AddAsync(MedicalRecord record);
    Task AddDocumentAsync(MedicalDocument document);
    Task UpdateAsync(MedicalRecord record);
    Task DeleteAsync(MedicalRecord record);
    Task SaveChangesAsync();
}