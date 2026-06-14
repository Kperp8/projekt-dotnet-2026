using Application.MedicalRecords.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.MedicalRecords.Mappers;

[Mapper]
public partial class MedicalRecordMapper
{
    /// <summary>
    /// MedicalRecordCreateDto -> nowa encja MedicalRecord.
    /// Id, Patient, Notes, CreatedAt i MedicalDocuments są ignorowane –
    /// Id generuje baza, CreatedAt ustawia serwis, resztę dodaje się osobno.
    /// </summary>
    [MapperIgnoreTarget(nameof(MedicalRecord.Id))]
    [MapperIgnoreTarget(nameof(MedicalRecord.Patient))]
    [MapperIgnoreTarget(nameof(MedicalRecord.Notes))]
    [MapperIgnoreTarget(nameof(MedicalRecord.CreatedAt))]
    [MapperIgnoreTarget(nameof(MedicalRecord.MedicalDocuments))]
    public partial MedicalRecord ToEntity(MedicalRecordCreateDto dto);

    /// <summary>
    /// MedicalRecordUpdateDto -> aktualizacja istniejącej encji MedicalRecord w miejscu.
    /// Niezmieniane pola (Id, PatientId, Patient, BloodType, CreatedAt, MedicalDocuments)
    /// są ignorowane – zachowujemy wartości z istniejącej encji.
    /// </summary>
    [MapperIgnoreTarget(nameof(MedicalRecord.Id))]
    [MapperIgnoreTarget(nameof(MedicalRecord.PatientId))]
    [MapperIgnoreTarget(nameof(MedicalRecord.Patient))]
    [MapperIgnoreTarget(nameof(MedicalRecord.BloodType))]
    [MapperIgnoreTarget(nameof(MedicalRecord.CreatedAt))]
    [MapperIgnoreTarget(nameof(MedicalRecord.MedicalDocuments))]
    public partial void UpdateEntity(MedicalRecordUpdateDto dto, [MappingTarget] MedicalRecord record);

    /// <summary>
    /// MedicalRecord -> MedicalRecordDetailsDto.
    /// PatientFullName jest obliczane ręcznie (konkatenacja FirstName + LastName).
    /// </summary>
    public MedicalRecordDetailsDto ToDetailsDto(MedicalRecord record)
    {
        var dto = ToDetailsDtoBase(record);
        dto.PatientFullName = $"{record.Patient?.FirstName} {record.Patient?.LastName}".Trim();
        return dto;
    }

    [MapperIgnoreSource(nameof(MedicalRecord.Patient))]
    [MapperIgnoreSource(nameof(MedicalRecord.MedicalDocuments))]
    [MapperIgnoreTarget(nameof(MedicalRecordDetailsDto.PatientFullName))]
    private partial MedicalRecordDetailsDto ToDetailsDtoBase(MedicalRecord record);

    /// <summary>
    /// MedicalRecord -> MedicalRecordListItemDto.
    /// PatientFullName jest obliczane ręcznie; pola szczegółowe są pomijane.
    /// </summary>
    public MedicalRecordListItemDto ToListItemDto(MedicalRecord record)
    {
        var dto = ToListItemDtoBase(record);
        dto.PatientFullName = $"{record.Patient?.FirstName} {record.Patient?.LastName}".Trim();
        return dto;
    }

    [MapperIgnoreSource(nameof(MedicalRecord.Patient))]
    [MapperIgnoreSource(nameof(MedicalRecord.Allergies))]
    [MapperIgnoreSource(nameof(MedicalRecord.ChronicDiseases))]
    [MapperIgnoreSource(nameof(MedicalRecord.Notes))]
    [MapperIgnoreSource(nameof(MedicalRecord.MedicalDocuments))]
    [MapperIgnoreTarget(nameof(MedicalRecordListItemDto.PatientFullName))]
    private partial MedicalRecordListItemDto ToListItemDtoBase(MedicalRecord record);

    /// <summary>Kolekcja MedicalRecord -> lista MedicalRecordListItemDto.</summary>
    public List<MedicalRecordListItemDto> ToListItemDtos(IEnumerable<MedicalRecord> records)
        => records.Select(ToListItemDto).ToList();
}