using Application.ClinicalNotes.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.ClinicalNotes.Mappers;

[Mapper]
public partial class ClinicalNotesMapper
{
    /// <summary>
    /// ClinicalNotesCreateDto -> nowa encja ClinicalNote.
    /// Id jest ignorowane – Id generuje baza.
    /// </summary>
    [MapperIgnoreTarget(nameof(ClinicalNote.Id))]
    public partial ClinicalNote ToEntity(ClinicalNotesCreateDto dto);

    /// <summary>
    /// ClinicalNote -> ClinicalNotesListItemDto.
    /// Pomijamy wszystko, ponieważ to potencjalne długie teksty.
    /// </summary>
    [MapperIgnoreSource(nameof(ClinicalNote.MedicalHistory))]
    [MapperIgnoreSource(nameof(ClinicalNote.Recommendations))]
    public partial ClinicalNotesListItemDto ToListItemDto(ClinicalNote note);

    /// <summary>
    /// ClinicalNote -> ClinicalNotesDetailsDto.
    /// </summary>
    public partial ClinicalNotesDetailsDto ToDetailsDto(ClinicalNote note);

/// <summary>Kolekcja ClinicalNote -> lista ClinicalNotesListItemDto.</summary>
    public List<ClinicalNotesListItemDto> ToListItemDtos(IEnumerable<ClinicalNote> notes)
        => notes.Select(ToListItemDto).ToList();
}