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

    public async Task<PatientListItemDto> FindPatientAsync(string name, string lastName)
    {
        Patient? patient;

        // jeśli 'name' ma 11 cyfr – traktujemy jako PESEL i ignorujemy lastName
        bool isPesel = name.Length == 11 && name.All(char.IsDigit);
        if (isPesel)
        {
            var results = await _repository.SearchAsync(name);
            patient = results.Count > 0
                ? await _repository.GetByIdAsync(results[0].Id)
                : null;
        }
        else
        {
            patient = await _repository.FindByNameAsync(name, lastName);
        }

        if (patient is null)
            throw new KeyNotFoundException($"Nie znaleziono pacjenta '{name} {lastName}'.");

        return _mapper.ToListItemDto(patient);
    }

    public async Task<IReadOnlyList<VisitListItemDto>> ListPatientVisitsAsync(string pesel)
    {
        var results = await _repository.SearchAsync(pesel);
        if (results.Count == 0)
            throw new KeyNotFoundException($"Nie znaleziono pacjenta z PESEL {pesel}.");

        var patient = await _repository.GetByIdAsync(results[0].Id);
        if (patient is null)
            throw new KeyNotFoundException($"Nie znaleziono pacjenta z PESEL {pesel}.");

        // TODO: zastąpić mapowaniem na VisitListItemDto gdy moduł Wizyt zostanie zaimplementowany
        return [];
    }
}
