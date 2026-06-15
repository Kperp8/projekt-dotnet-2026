using Application.Procedure.Dtos;

namespace Application.Procedure.Interfaces;

public interface IProceduresService
{
    /// <summary>Tworzy nową procedurę na podstawie danych z formularza.</summary>
    Task<ProceduresDetailsDto> CreateAsync(ProceduresCreateDto dto);

    /// <summary>Zwraca szczegóły procedury lub <c>null</c>, jeśli procedura nie istnieje.</summary>
    Task<ProceduresDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę procedur. Strony numerowane od 1.</summary>
    Task<IReadOnlyList<ProceduresListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje procedury po frazie (nazwa, opis). Zwraca pustą listę dla pustego zapytania.</summary>
    Task<IReadOnlyList<ProceduresListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje opis i cenę. Rzuca <see cref="KeyNotFoundException"/>, jeśli wizyta nie istnieje.</summary>
    Task<ProceduresDetailsDto> UpdateAsync(int id, ProceduresUpdateDto dto);

    /// <summary>
    /// Trwale usuwa procedurę.
    /// Rzuca <see cref="KeyNotFoundException"/>, jeśli wizyta nie istnieje.
    /// </summary>
    Task DeleteAsync(int id);
}