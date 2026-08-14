using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Interfaces;
using Application.ClinicalNotes.Mappers;
using Domain.ClinicalNotes;
using Microsoft.Extensions.Logging;

namespace Application.ClinicalNotes.Services;

public class ClinicalNotesService : IClinicalNotesService
{
    private readonly IClinicalNotesRepository _repository;
    private readonly ClinicalNotesMapper _mapper;
    private readonly ILogger<ClinicalNotesService> _logger;

    public ClinicalNotesService(IClinicalNotesRepository repository, ClinicalNotesMapper mapper, ILogger<ClinicalNotesService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ClinicalNotesDetailsDto> CreateAsync(ClinicalNotesCreateDto dto)
    {
        var note = _mapper.ToEntity(dto);

        await _repository.AddAsync(note);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono notatkę kliniczną: Id={Id}", note.Id);

        return _mapper.ToDetailsDto(note);
    }

    public async Task<ClinicalNotesDetailsDto?> GetByIdAsync(int id)
    {
        var note = await _repository.GetByIdAsync(id);
        return note is null ? null : _mapper.ToDetailsDto(note);
    }

    public async Task<IReadOnlyList<ClinicalNotesListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var notes = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(notes);
    }

    public async Task<IReadOnlyList<ClinicalNotesListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var notes = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(notes);
    }

    public async Task<ClinicalNotesDetailsDto> UpdateAsync(int id, ClinicalNotesUpdateDto dto)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note is null)
        {
            throw new KeyNotFoundException($"Notatka o Id={id} nie istnieje.");
        }

        // Częściowa aktualizacja – nadpisujemy tylko przekazane wartości
        if (dto.MedicalHistory is not null) note.MedicalHistory = dto.MedicalHistory;
        if (dto.Diagnosis is not null) note.Diagnosis = dto.Diagnosis;
        if (dto.Recommendations is not null) note.Recommendations = dto.Recommendations;

        await _repository.UpdateAsync(note);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano notatkę kliniczną Id={Id}", note.Id);

        return _mapper.ToDetailsDto(note);
    }

    public async Task DeleteAsync(int id)
    {
        var note = await _repository.GetByIdAsync(id);
        if (note is null)
        {
            throw new KeyNotFoundException($"Notatka o Id={id} nie istnieje.");
        }

        await _repository.DeleteAsync(note);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Usunięto notatkę Id={Id}", id);
    }
}