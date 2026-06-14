using Application.Patients.Dtos;
using Application.Visits.Dtos;

namespace Application.Patients.Interfaces;

public interface IPatientService
{
    /// <summary>Tworzy nowego pacjenta. Rzuca wyjątek jeśli PESEL już istnieje.</summary>
    Task<PatientDetailsDto> CreateAsync(PatientCreateDto dto);

    /// <summary>Zwraca pacjenta po Id. Zwraca null jeśli nie istnieje lub jest usunięty.</summary>
    Task<PatientDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę pacjentów (bez usuniętych).</summary>
    Task<IReadOnlyList<PatientListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje pacjentów po fragmencie nazwiska lub dokładnym PESEL.</summary>
    Task<IReadOnlyList<PatientListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje dane pacjenta. Rzuca wyjątek jeśli pacjent nie istnieje.</summary>
    Task<PatientDetailsDto> UpdateAsync(int id, PatientUpdateDto dto);

    /// <summary>Miękkie usunięcie pacjenta (soft delete). Rzuca wyjątek jeśli pacjent nie istnieje.</summary>
    Task DeleteAsync(int id);

    /// <summary>Wyszukiwanie pacjenta po nazwisku/nr PESEL. Rzuca wyjątek jeśli pacjent nie istnieje.</summary>
    Task<PatientListItemDto> FindPatientAsync(string name, string lastName);

    /// <summary>Zwraca listę wizyt pacjenta po PESEL. Rzuca wyjątek jeśli pacjent nie istnieje.</summary>
    Task<IReadOnlyList<VisitListItemDto>> ListPatientVisitsAsync(string pesel);
}
