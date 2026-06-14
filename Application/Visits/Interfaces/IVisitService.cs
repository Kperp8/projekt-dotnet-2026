using Application.Visits.Dtos;

namespace Application.Visits.Interfaces;

public interface IVisitService
{
    /// <summary>Tworzy nową wizytę na podstawie danych z formularza.</summary>
    Task<VisitDetailsDto> CreateAsync(VisitCreateDto dto);

    /// <summary>Zwraca szczegóły wizyty lub <c>null</c>, jeśli wizyta nie istnieje.</summary>
    Task<VisitDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę wizyt. Strony numerowane od 1.</summary>
    Task<IReadOnlyList<VisitListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje wizyty po frazie (nazwisko pacjenta, PESEL). Zwraca pustą listę dla pustego zapytania.</summary>
    Task<IReadOnlyList<VisitListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje termin, status i przypisanego lekarza. Rzuca <see cref="KeyNotFoundException"/>, jeśli wizyta nie istnieje.</summary>
    Task<VisitDetailsDto> UpdateAsync(int id, VisitUpdateDto dto);

    /// <summary>
    /// Anuluje wizytę (soft-delete – ustawia status <c>Cancelled</c>).
    /// Rzuca <see cref="KeyNotFoundException"/>, jeśli wizyta nie istnieje.
    /// Rzuca <see cref="InvalidOperationException"/>, jeśli wizyta jest już zakończona.
    /// </summary>
    Task DeleteAsync(int id);
}