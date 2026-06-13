using Application.Visits.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Visits.Mappers;

[Mapper]
public partial class VisitMapper
{
    // VisitCreateDto -> nowa encja Visit.
    // Id i Patient są ignorowane – Id generuje baza, Patient ustawia serwis przez PatientId.
    // Kolekcje procedur i notatek są ignorowane – dodawane osobnymi endpointami po utworzeniu wizyty.
    [MapperIgnoreTarget(nameof(Visit.Id))]
    [MapperIgnoreTarget(nameof(Visit.Patient))]
    [MapperIgnoreTarget(nameof(Visit.ProceduresPerformed))]
    [MapperIgnoreTarget(nameof(Visit.ClinicalNotes))]
    public partial Visit ToEntity(VisitCreateDto dto);

    // VisitUpdateDto -> aktualizacja istniejącej encji Visit w miejscu.
    // Id, PatientId i Patient są zachowane z istniejącej encji – pacjenta nie można zmienić po utworzeniu wizyty.
    [MapperIgnoreTarget(nameof(Visit.Id))]
    [MapperIgnoreTarget(nameof(Visit.PatientId))]
    [MapperIgnoreTarget(nameof(Visit.Patient))]
    [MapperIgnoreTarget(nameof(Visit.ProceduresPerformed))]
    [MapperIgnoreTarget(nameof(Visit.ClinicalNotes))]
    public partial void UpdateEntity(VisitUpdateDto dto, [MappingTarget] Visit visit);

    // Visit -> VisitListItemDto.
    // PatientId mapuje się przez konwencję (ta sama nazwa). PatientFullName jest obliczane ręcznie –
    // Mapperly nie skleja stringów z dwóch właściwości; ustawiamy je w ToListItemDto poniżej.
    // Patient, ProceduresPerformed i ClinicalNotes nie mają odpowiedników w tym DTO.
    [MapperIgnoreSource(nameof(Visit.Patient))]
    [MapperIgnoreSource(nameof(Visit.ProceduresPerformed))]
    [MapperIgnoreSource(nameof(Visit.ClinicalNotes))]
    [MapperIgnoreTarget(nameof(VisitListItemDto.PatientFullName))]
    public partial VisitListItemDto ToListItemDtoPartial(Visit visit);

    // Visit -> VisitDetailsDto.
    // PatientFullName i kolekcje (ProceduresPerformed, ClinicalNotes) są ignorowane –
    // ustawiane przez serwis po zmapowaniu (kolekcje wymagają własnych DTO, jeszcze niezdefiniowanych).
    [MapperIgnoreSource(nameof(Visit.Patient))]
    [MapperIgnoreSource(nameof(Visit.ProceduresPerformed))]
    [MapperIgnoreSource(nameof(Visit.ClinicalNotes))]
    [MapperIgnoreTarget(nameof(VisitDetailsDto.PatientFullName))]
    [MapperIgnoreTarget(nameof(VisitDetailsDto.ProceduresPerformed))]
    [MapperIgnoreTarget(nameof(VisitDetailsDto.ClinicalNotes))]
    public partial VisitDetailsDto ToDetailsDtoPartial(Visit visit);

    // Publiczne metody z uzupełnianiem PatientFullName, który nie może być zmapowany przez Mapperly.
    public VisitListItemDto ToListItemDto(Visit visit)
    {
        var dto = ToListItemDtoPartial(visit);
        dto.PatientFullName = $"{visit.Patient.FirstName} {visit.Patient.LastName}";
        return dto;
    }

    public VisitDetailsDto ToDetailsDto(Visit visit)
    {
        var dto = ToDetailsDtoPartial(visit);
        dto.PatientFullName = $"{visit.Patient.FirstName} {visit.Patient.LastName}";
        return dto;
    }

    // Kolekcja Visit -> lista VisitListItemDto.
    public List<VisitListItemDto> ToListItemDtos(IEnumerable<Visit> visits)
        => visits.Select(ToListItemDto).ToList();
}