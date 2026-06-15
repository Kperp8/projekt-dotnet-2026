using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Application.Procedures.Mappers;
using Domain.Procedure;
using Domain.Procedures;
using Microsoft.Extensions.Logging;

namespace Application.Procedures.Services;

public class ProceduresService : IProceduresService
{
    private readonly IProceduresRepository _repository;
    private readonly IProceduresMapper _mapper;
    private readonly ILogger<ProceduresService> _logger;

    public ProceduresService(IProceduresRepository repository, IProceduresMapper mapper, ILogger<ProceduresService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProceduresDetailsDto> CreateAsync(ProceduresCreateDto dto)
    {
        var procedure = _mapper.ToEntity(dto);

        await _repository.AddAsync(procedure);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono procedurę: Id={Id}, Nazwa={ProcedureName}", procedure.Id, procedure.ProcedureName);

        return _mapper.ToDetailsDto(procedure);
    }

    public async Task<ProceduresDetailsDto?> GetByIdAsync(int id)
    {
        var procedure = await _repository.GetByIdAsync(id);
        return procedure is null ? null : _mapper.ToDetailsDto(procedure);
    }

    public async Task<IReadOnlyList<ProceduresListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var procedures = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(procedures);
    }

    public async Task<IReadOnlyList<ProceduresListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var procedures = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(procedures);
    }

    public async Task<ProceduresDetailsDto> UpdateAsync(int id, ProceduresUpdateDto dto)
    {
        var procedure = await _repository.GetByIdAsync(id);
        if (procedure is null)
        {
            throw new KeyNotFoundException($"Procedura o Id={id} nie istnieje.");
        }

        _mapper.UpdateEntity(dto, procedure);

        await _repository.UpdateAsync(procedure);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano procedurę Id={Id}, nowy opis={Description}, nowa cena={Price}", procedure.Id, procedure.Description, procedure.Price);

        return _mapper.ToDetailsDto(procedure);
    }

    public async Task DeleteAsync(int id)
    {
        var procedure = await _repository.GetByIdAsync(id);
        if (procedure is null)
        {
            throw new KeyNotFoundException($"Procedura o Id={id} nie istnieje.");
        }

        await _repository.DeleteAsync(procedure);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Usunięto procedurę Id={Id}", id);
    }
}