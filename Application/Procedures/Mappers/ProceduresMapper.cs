using Application.Procedures.Dtos;
using Riok.Mapperly.Abstractions;

namespace Application.Procedures.Mappers;

[Mapper]
public partial class ProceduresMapper
{
    /// <summary>
    /// ProceduresCreateDto -> nowa encja Procedure.
    /// Id jest ignorowane – Id generuje baza.
    /// </summary>
    [MapperIgnoreTarget(nameof(Procedure.Id))]
    public partial Procedure ToEntity(ProceduresCreateDto dto);

    /// <summary>
    /// Procedure -> ProceduresListItemDto.
    /// Pomijamy Description – nie jest potrzebny na liście.
    /// </summary>
    [MapperIgnoreSource(nameof(Procedure.Description))]
    public partial ProceduresListItemDto ToListItemDto(Procedure procedure);

    /// <summary>
    /// Procedure -> ProceduresDetailsDto.
    /// </summary>
    public partial ProceduresDetailsDto ToDetailsDto(Procedure procedure);

    /// <summary>Kolekcja Procedure -> lista ProceduresListItemDto.</summary>
    public List<ProceduresListItemDto> ToListItemDtos(IEnumerable<Procedure> procedures)
        => procedures.Select(ToListItemDto).ToList();
}