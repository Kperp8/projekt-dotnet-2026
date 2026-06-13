using Application.Patients.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Patients.Mappers;

[Mapper]
public partial class PatientMapper
{
    // PatientCreateDto -> nowa encja Patient.
    // Id, IsDeleted i Visits są ignorowane – Id generuje baza, resztę ustawia serwis osobno.
    [MapperIgnoreTarget(nameof(Patient.Id))]
    [MapperIgnoreTarget(nameof(Patient.IsDeleted))]
    [MapperIgnoreTarget(nameof(Patient.Visits))]
    public partial Patient ToEntity(PatientCreateDto dto);

    // PatientUpdateDto -> aktualizacja istniejącej encji Patient w miejscu.
    // Id, Pesel, IsDeleted i Visits nie są w DTO – zachowujemy wartości z istniejącej encji.
    [MapperIgnoreTarget(nameof(Patient.Id))]
    [MapperIgnoreTarget(nameof(Patient.Pesel))]
    [MapperIgnoreTarget(nameof(Patient.IsDeleted))]
    [MapperIgnoreTarget(nameof(Patient.Visits))]
    public partial void UpdateEntity(PatientUpdateDto dto, [MappingTarget] Patient patient);

    // Patient -> PatientDetailsDto.
    // IsDeleted jest polem technicznym – nie wystawiamy go przez API.
    [MapProperty("Visits.Count", nameof(PatientDetailsDto.VisitsCount))]
    [MapperIgnoreSource(nameof(Patient.IsDeleted))]
    public partial PatientDetailsDto ToDetailsDto(Patient patient);

    // Patient -> PatientListItemDto.
    // Dane kontaktowe i techniczne są pomijane – DTO jest lekkie, tylko do listy.
    [MapProperty("Visits.Count", nameof(PatientListItemDto.VisitsCount))]
    [MapperIgnoreSource(nameof(Patient.InsuranceNumber))]
    [MapperIgnoreSource(nameof(Patient.PhoneNumber))]
    [MapperIgnoreSource(nameof(Patient.Email))]
    [MapperIgnoreSource(nameof(Patient.Address))]
    [MapperIgnoreSource(nameof(Patient.IsDeleted))]
    public partial PatientListItemDto ToListItemDto(Patient patient);

    // Kolekcja Patient -> lista PatientListItemDto.
    public List<PatientListItemDto> ToListItemDtos(IEnumerable<Patient> patients)
        => patients.Select(ToListItemDto).ToList();
}
