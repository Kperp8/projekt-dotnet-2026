using Application.Procedure.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Procedure.Mappers;

[Mapper]
public partial class ProceduresMapper
{
    /// <summary>
    /// ProceduresCreateDto -> nowa encja Procedures.
    /// Id jest jest ignorowane – Id generuje baza.
    /// </summary>
    [MapperIgnoreTarget(nameof(Procedures.Id))]
    public partial Procedures ToEntity(ProceduresCreateDto dto);

    /// <summary>
    /// VisitUpdateDto -> aktualizacja istniejącej encji Procedures w miejscu.
    /// Id i ProcedureName są zachowane z istniejącej encji.
    /// </summary>
    [MapperIgnoreTarget(nameof(Procedures.Id))]
    [MapperIgnoreTarget(nameof(Procedures.ProcedureName))]
    public partial void UpdateEntity(ProceduresUpdateDto dto, [MappingTarget] Procedures procedure);

    /// <summary>
    /// Procedures -> ProceduresListItemDto.
    /// Pomijamy jedynie Description
    /// </summary>
    [MapperIgnoreSource(nameof(Procedures.Description))]
    public partial ProceduresListItemDto ToListItemDto(Procedures procedure);

    /// <summary>
    /// Procedures -> ProceduresDetailsDto.
    /// </summary>
    public partial ProceduresDetailsDto ToDetailsDto(Procedures procedure);

    /// <summary>Kolekcja Procedures -> lista ProceduresListItemDto.</summary>
    public List<ProceduresListItemDto> ToListItemDtos(IEnumerable<Procedures> procedures)
        => procedures.Select(ToListItemDto).ToList();
}