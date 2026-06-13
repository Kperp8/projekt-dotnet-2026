using Application.Visits.Dtos;
using Application.Visits.Interfaces;
using Application.Visits.Mappers;
using Domain.Visits;
using Microsoft.Extensions.Logging;

namespace Application.Visits.Services;

public class VisitService : IVisitService
{
    private readonly IVisitsRepository _repository;
    private readonly VisitMapper _mapper;
    private readonly ILogger<VisitService> _logger;

    public VisitService(IVisitsRepository repository, VisitMapper mapper, ILogger<VisitService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<VisitDetailsDto> CreateAsync(VisitCreateDto dto)
    {
        var visit = _mapper.ToEntity(dto);

        await _repository.AddAsync(visit);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono wizytę: Id={Id}, PatientId={PatientId}", visit.Id, visit.PatientId);

        // Wczytujemy ponownie, żeby mieć nawigację Patient uzupełnioną przez EF Core.
        var created = await _repository.GetByIdAsync(visit.Id);
        return _mapper.ToDetailsDto(created!);
    }

    public async Task<VisitDetailsDto?> GetByIdAsync(int id)
    {
        var visit = await _repository.GetByIdAsync(id);
        return visit is null ? null : _mapper.ToDetailsDto(visit);
    }

    public async Task<IReadOnlyList<VisitListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var visits = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(visits);
    }

    public async Task<IReadOnlyList<VisitListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var visits = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(visits);
    }

    public async Task<VisitDetailsDto> UpdateAsync(int id, VisitUpdateDto dto)
    {
        var visit = await _repository.GetByIdAsync(id);
        if (visit is null)
        {
            throw new KeyNotFoundException($"Wizyta o Id={id} nie istnieje.");
        }

        _mapper.UpdateEntity(dto, visit);

        await _repository.UpdateAsync(visit);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano wizytę Id={Id}, nowy status={Status}", visit.Id, visit.Status);

        return _mapper.ToDetailsDto(visit);
    }

    public async Task DeleteAsync(int id)
    {
        var visit = await _repository.GetByIdAsync(id);
        if (visit is null)
        {
            throw new KeyNotFoundException($"Wizyta o Id={id} nie istnieje.");
        }

        // Wizyty nie są fizycznie usuwane – anulowanie jest odpowiednikiem soft-delete.
        // Zakończone wizyty są częścią dokumentacji medycznej i nie powinny znikać z bazy.
        if (visit.Status == VisitStatus.Completed)
        {
            throw new InvalidOperationException($"Nie można anulować zakończonej wizyty (Id={id}).");
        }

        visit.Status = VisitStatus.Cancelled;

        await _repository.UpdateAsync(visit);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Anulowano wizytę Id={Id}", visit.Id);
    }
}