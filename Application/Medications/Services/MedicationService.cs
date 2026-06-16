using Application.Medications.Dtos;
using Application.Medications.Interfaces;
using Application.Medications.Mappers;
using Domain.Medications;
using Microsoft.Extensions.Logging;

namespace Application.Medications.Services;

public class MedicationService : IMedicationService
{
    private readonly IMedicationsRepository _repository;
    private readonly MedicationMapper _mapper;
    private readonly ILogger<MedicationService> _logger;

    public MedicationService(IMedicationsRepository repository, MedicationMapper mapper, ILogger<MedicationService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MedicationDetailsDto> CreateAsync(MedicationCreateDto dto)
    {
        var medication = _mapper.ToEntity(dto);

        await _repository.AddAsync(medication);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono lek: Id={Id}, Nazwa={Name}", medication.Id, medication.Name);

        return _mapper.ToDetailsDto(medication);
    }

    public async Task<MedicationDetailsDto?> GetByIdAsync(int id)
    {
        var medication = await _repository.GetByIdAsync(id);
        return medication is null ? null : _mapper.ToDetailsDto(medication);
    }

    public async Task<IReadOnlyList<MedicationListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var medications = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(medications);
    }

    public async Task<IReadOnlyList<MedicationListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var medications = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(medications);
    }

    public async Task<MedicationDetailsDto> UpdateAsync(int id, MedicationUpdateDto dto)
    {
        var medication = await _repository.GetByIdAsync(id);
        if (medication is null)
        {
            throw new KeyNotFoundException($"Lek o Id={id} nie istnieje.");
        }

        // Częściowa aktualizacja – nadpisujemy tylko przekazane wartości
        if (dto.Dosing is not null) medication.Dosing = dto.Dosing;
        medication.Cost = dto.Cost;

        await _repository.UpdateAsync(medication);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano lek Id={Id}", medication.Id);

        return _mapper.ToDetailsDto(medication);
    }

    public async Task DeleteAsync(int id)
    {
        var medication = await _repository.GetByIdAsync(id);
        if (medication is null)
        {
            throw new KeyNotFoundException($"Lek o Id={id} nie istnieje.");
        }

        await _repository.DeleteAsync(medication);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Usunięto lek Id={Id}", id);
    }
}