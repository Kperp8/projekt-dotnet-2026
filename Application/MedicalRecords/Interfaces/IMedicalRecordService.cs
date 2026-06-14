using Application.MedicalRecords.Dtos;

namespace Application.MedicalRecords.Interfaces;

public interface IMedicalRecordService
{
    /// <summary>Tworzy nowy pacjenta.</summary>
    Task<MedicalRecordDetailsDto> CreateAsync(MedicalRecordCreateDto dto);

    /// <summary>Zwraca rekord po Id. Zwraca null jeśli nie istnieje lub jest usunięty.</summary>
    Task<MedicalRecordDetailsDto?> GetByIdAsync(int id);

    /// <summary>Zwraca stronicowaną listę rekordów.</summary>
    Task<IReadOnlyList<MedicalRecordListItemDto>> GetPagedAsync(int page, int pageSize);

    /// <summary>Wyszukuje rekordy po frazie (nazwisko pacjenta, pesel).</summary>
    Task<IReadOnlyList<MedicalRecordListItemDto>> SearchAsync(string query);

    /// <summary>Aktualizuje dane rekordu. Rzuca wyjątek jeśli rekord nie istnieje.</summary>
    Task<MedicalRecordDetailsDto> UpdateAsync(int id, MedicalRecordUpdateDto dto);

    /// <summary>Trwale usuwa rekord medyczny. Rzuca wyjątek jeśli rekord nie istnieje.</summary>
    Task DeleteAsync(int id);

    /// <summary>Zapisuje plik skanu i dodaje wpis dokumentu do rekordu medycznego.</summary>
    Task<MedicalDocumentDto> UploadDocumentAsync(UploadMedicalDocumentDto dto);
}