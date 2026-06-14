using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Interfaces;
using Application.MedicalRecords.Mappers;
using Domain.MedicalRecords;
using Microsoft.Extensions.Logging;

namespace Application.MedicalRecords.Services;

public class MedicalRecordService : IMedicalRecordService
{
    private readonly IMedicalRecordsRepository _repository;
    private readonly MedicalRecordMapper _mapper;
    private readonly ILogger<MedicalRecordService> _logger;

    public MedicalRecordService(IMedicalRecordsRepository repository, MedicalRecordMapper mapper, ILogger<MedicalRecordService> logger)
    {
        _repository = repository;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<MedicalRecordDetailsDto> CreateAsync(MedicalRecordCreateDto dto)
    {
        var record = _mapper.ToEntity(dto);
        record.CreatedAt = DateTime.UtcNow;

        await _repository.AddAsync(record);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Utworzono rekord medyczny: Id={Id}, PatientId={PatientId}", record.Id, record.PatientId);

        return _mapper.ToDetailsDto(record);
    }

    public async Task<MedicalRecordDetailsDto?> GetByIdAsync(int id)
    {
        var record = await _repository.GetByIdAsync(id);

        if (record is null)
        {
            return null;
        }

        return _mapper.ToDetailsDto(record);
    }

    public async Task<IReadOnlyList<MedicalRecordListItemDto>> GetPagedAsync(int page, int pageSize)
    {
        var records = await _repository.GetPagedAsync(page, pageSize);
        return _mapper.ToListItemDtos(records);
    }

    public async Task<IReadOnlyList<MedicalRecordListItemDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            return [];
        }

        var records = await _repository.SearchAsync(query.Trim());
        return _mapper.ToListItemDtos(records);
    }

    public async Task<MedicalRecordDetailsDto> UpdateAsync(int id, MedicalRecordUpdateDto dto)
    {
        var record = await _repository.GetByIdAsync(id);
        if (record is null)
        {
            throw new KeyNotFoundException($"Rekord medyczny o Id={id} nie istnieje.");
        }

        _mapper.UpdateEntity(dto, record);

        await _repository.UpdateAsync(record);
        await _repository.SaveChangesAsync();

        _logger.LogInformation("Zaktualizowano rekord medyczny Id={Id}", record.Id);

        return _mapper.ToDetailsDto(record);
    }
}