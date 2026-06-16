using Application.Procedures.Dtos;
using Application.Procedures.Interfaces;
using Application.Procedures.Mappers;
using Domain.Procedures;
using Microsoft.Extensions.Logging;

namespace Application.Procedures.Services;

public class ProceduresService : IProceduresService
{
    private readonly IProceduresRepository _repository;
    private readonly ProceduresMapper _mapper;
    private readonly ILogger<ProceduresService> _logger;

    public ProceduresService(IProceduresRepository repository, ProceduresMapper mapper, ILogger<ProceduresService> logger)
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

        _logger.LogInformation("Utworzono procedurę: Id={Id}, Nazwa={Name}", procedure.Id, procedure.Name);

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

        // Częściowa aktualizacja – nadpisujemy tylko przekazane wartości
        if (dto.Name is not null)        procedure.Name = dto.Name;
        if (dto.Description is not null) procedure.Description = dto.Description;
        if (dto.Price is not null)       procedure.Price = dto.Price.Value;

        await _repository.UpdateAsync(procedure);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano procedurę Id={Id}", procedure.Id);

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