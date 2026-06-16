using Application.Medications.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Medications.Mappers;

[Mapper]
public partial class MedicationMapper
{
    /// <summary>
    /// MedicationCreateDto -> nowa encja Medication.
    /// Id i Quantity są ignorowane – Id generuje baza, Quantity ustawia serwis.
    /// </summary>
    [MapperIgnoreTarget(nameof(Medication.Id))]
    [MapperIgnoreTarget(nameof(Medication.Quantity))]
    public partial Medication ToEntity(MedicationCreateDto dto);

    /// <summary>
    /// MedicationUpdateDto -> aktualizacja istniejącej encji Medication w miejscu.
    /// Niezmieniane pola (Id, Name, Patient, Quantity)
    /// są ignorowane – zachowujemy wartości z istniejącej encji.
    /// </summary>
    [MapperIgnoreTarget(nameof(Medication.Id))]
    [MapperIgnoreTarget(nameof(Medication.Name))]
    [MapperIgnoreTarget(nameof(Medication.Quantity))]
    public partial void UpdateEntity(MedicationsUpdateDto dto, [MappingTarget] Medication medication);

    /// <summary>
    /// Medication -> MedicationListItemDto.
    /// Pomijamy Dosing – nie jest potrzebny na liście.
    /// </summary>
    [MapperIgnoreSource(nameof(Medication.Dosing))]
    public partial MedicationListItemDto ToListItemDto(Medication medication);

    /// <summary>
    /// Medication -> MedicationDetailsDto.
    /// </summary>
    public partial MedicationsDetailsDto ToDetailsDto(Medication medication);

    /// <summary>Kolekcja Medication -> lista MedicationListItemDto.</summary>
    public List<MedicationListItemDto> ToListItemDtos(IEnumerable<Medication> medications)
        => medications.Select(ToListItemDto).ToList();
}