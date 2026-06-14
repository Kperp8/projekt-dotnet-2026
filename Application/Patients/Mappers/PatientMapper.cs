using Application.Patients.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Patients.Mappers;

[Mapper]
public partial class PatientMapper
{
    /// <summary>
    /// PatientCreateDto -> nowa encja Patient.
    /// Id, IsDeleted i Visits są ignorowane – Id generuje baza, resztę ustawia serwis osobno.
    /// </summary>
    [MapperIgnoreTarget(nameof(Patient.Id))]
    [MapperIgnoreTarget(nameof(Patient.IsDeleted))]
    [MapperIgnoreTarget(nameof(Patient.Visits))]
    public partial Patient ToEntity(PatientCreateDto dto);

    /// <summary>
    /// PatientUpdateDto -> aktualizacja istniejącej encji Patient w miejscu.
    /// Id, Pesel, IsDeleted i Visits nie są w DTO – zachowujemy wartości z istniejącej encji.
    /// </summary>
    [MapperIgnoreTarget(nameof(Patient.Id))]
    [MapperIgnoreTarget(nameof(Patient.Pesel))]
    [MapperIgnoreTarget(nameof(Patient.IsDeleted))]
    [MapperIgnoreTarget(nameof(Patient.Visits))]
    public partial void UpdateEntity(PatientUpdateDto dto, [MappingTarget] Patient patient);

    /// <summary>
    /// Patient -> PatientDetailsDto.
    /// IsDeleted jest polem technicznym – nie wystawiamy go przez API.
    /// </summary>
    [MapProperty("Visits.Count", nameof(PatientDetailsDto.VisitsCount))]
    [MapperIgnoreSource(nameof(Patient.IsDeleted))]
    public partial PatientDetailsDto ToDetailsDto(Patient patient);

    /// <summary>
    /// Patient -> PatientListItemDto.
    /// Dane kontaktowe i techniczne są pomijane – DTO jest lekkie, tylko do listy.
    /// </summary>
    [MapProperty("Visits.Count", nameof(PatientListItemDto.VisitsCount))]
    [MapperIgnoreSource(nameof(Patient.InsuranceNumber))]
    [MapperIgnoreSource(nameof(Patient.PhoneNumber))]
    [MapperIgnoreSource(nameof(Patient.Email))]
    [MapperIgnoreSource(nameof(Patient.Address))]
    [MapperIgnoreSource(nameof(Patient.IsDeleted))]
    public partial PatientListItemDto ToListItemDto(Patient patient);

    /// <summary>Kolekcja Patient -> lista PatientListItemDto.</summary>
    public List<PatientListItemDto> ToListItemDtos(IEnumerable<Patient> patients)
        => patients.Select(ToListItemDto).ToList();
}
