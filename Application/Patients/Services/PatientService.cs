using Application.Patients.Dtos;
using Application.Patients.Interfaces;
using Application.Patients.Mappers;
using Domain.Patients;
using Microsoft.Extensions.Logging;

namespace Application.Patients.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _repository;
    private readonly PatientMapper _mapper;
    private readonly ILogger<PatientService> _logger;

    public PatientService(IPatientRepository repository, PatientMapper mapper, ILogger<PatientService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PatientDetailsDto> CreateAsync(PatientCreateDto dto)
    {
        bool exists = await _repository.ExistsByPeselAsync(dto.Pesel);
        if (exists)
        {
            _logger.LogWarning("Próba utworzenia pacjenta z istniejącym PESEL: {Pesel}", dto.Pesel);
            throw new InvalidOperationException($"Pacjent z PESEL {dto.Pesel} już istnieje.");
        }

        var patient = _mapper.ToEntity(dto);

        await _repository.AddAsync(patient);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono nowego pacjenta: Id={Id}, PESEL={Pesel}", patient.Id, patient.Pesel);

        return _mapper.ToDetailsDto(patient);
    }

    public async Task<PatientDetailsDto?> GetByIdAsync(int id)
    {
        var patient = await _repository.GetByIdAsync(id);

        if (patient is null)
        {
            return null;
        }

        return _mapper.ToDetailsDto(patient);
    }

    public async Task<IReadOnlyList<PatientListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var patients = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(patients);
    }

    public async Task<IReadOnlyList<PatientListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var patients = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(patients);
    }

    public async Task<PatientDetailsDto> UpdateAsync(int id, PatientUpdateDto dto)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null)
        {
            throw new KeyNotFoundException($"Pacjent o Id={id} nie istnieje.");
        }

        _mapper.UpdateEntity(dto, patient);

        await _repository.UpdateAsync(patient);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano pacjenta Id={Id}", patient.Id);

        return _mapper.ToDetailsDto(patient);
    }

    public async Task DeleteAsync(int id)
    {
        var patient = await _repository.GetByIdAsync(id);
        if (patient is null)
        {
            throw new KeyNotFoundException($"Pacjent o Id={id} nie istnieje.");
        }

        patient.IsDeleted = true;

        await _repository.UpdateAsync(patient);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Usunięto (soft delete) pacjenta Id={Id}", patient.Id);
    }
}
