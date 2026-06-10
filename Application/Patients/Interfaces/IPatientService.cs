using Application.Patients.Dtos;

namespace Application.Patients.Interfaces;

public interface IPatientService
{
    /// Tworzy nowego pacjenta. Rzuca wyjątek jeśli PESEL już istnieje.
    Task<PatientDetailsDto> CreateAsync(PatientCreateDto dto);

    /// Zwraca pacjenta po Id. Zwraca null jeśli nie istnieje lub jest usunięty.
    Task<PatientDetailsDto?> GetByIdAsync(int id);

    /// Zwraca stronicowaną listę pacjentów (bez usuniętych).
    Task<IReadOnlyList<PatientListItemDto>> GetPagedAsync(int page, int pageSize);

    /// Wyszukuje pacjentów po fragmencie nazwiska lub dokładnym PESEL.
    Task<IReadOnlyList<PatientListItemDto>> SearchAsync(string query);

    /// Aktualizuje dane pacjenta. Rzuca wyjątek jeśli pacjent nie istnieje.
    Task<PatientDetailsDto> UpdateAsync(int id, PatientUpdateDto dto);

    /// Miękkie usunięcie pacjenta (soft delete). Rzuca wyjątek jeśli pacjent nie istnieje.
    Task DeleteAsync(int id);
}
