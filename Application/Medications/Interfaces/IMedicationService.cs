using Application.Medications.Dtos;

namespace Application.Medications.Interfaces;

public interface IMedicationService
{
    /// <summary>Tworzy nowy lek.</summary>
    Task<MedicationDetailsDto> CreateAsync(MedicationCreateDto dto);

    /// <summary>Zwraca lek po Id. Zwraca null jeśli nie istnieje lub jest usunięty.</summary>
    Task<MedicationDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę leków.</summary>
    Task<IReadOnlyList<MedicationListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje leki po frazie (nazwa, dozowanie, ilość, cena).</summary>
    Task<IReadOnlyList<MedicationListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje dane leku. Rzuca wyjątek jeśli rekord nie istnieje.</summary>
    Task<MedicationDetailsDto> UpdateAsync(int id, MedicationUpdateDto dto);

    /// <summary>Trwale usuwa lek. Rzuca wyjątek jeśli lek nie istnieje.</summary>
    Task DeleteAsync(int id);
}