using Application.ClinicalNotes.Dtos;

namespace Application.ClinicalNotes.Interfaces;

public interface IClinicalNotesService
{
    /// <summary>Tworzy nową notatkę na podstawie danych z formularza.</summary>
    Task<ClinicalNotesDetailsDto> CreateAsync(ClinicalNotesCreateDto dto);

    /// <summary>Zwraca szczegóły notatki lub <c>null</c>, jeśli notatka nie istnieje.</summary>
    Task<ClinicalNotesDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę notatek. Strony numerowane od 1.</summary>
    Task<IReadOnlyList<ClinicalNotesListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje notatki po frazie (nazwa, opis). Zwraca pustą listę dla pustego zapytania.</summary>
    Task<IReadOnlyList<ClinicalNotesListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje notatkę. Rzuca <see cref="KeyNotFoundException"/>, jeśli notatka nie istnieje.</summary>
    Task<ClinicalNotesDetailsDto> UpdateAsync(int id, ClinicalNotesUpdateDto dto);

    /// <summary>
    /// Trwale usuwa notatkę.
    /// Rzuca <see cref="KeyNotFoundException"/>, jeśli notatka nie istnieje.
    /// </summary>
    Task DeleteAsync(int id);
}