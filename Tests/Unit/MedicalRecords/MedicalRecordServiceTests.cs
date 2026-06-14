using Application.MedicalRecords.Dtos;
using Application.MedicalRecords.Mappers;
using Application.MedicalRecords.Services;
using Domain.MedicalRecords;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.MedicalRecords;

[TestFixture]
public class MedicalRecordServiceTests
{
    private IMedicalRecordsRepository _repository = null!;
    private MedicalRecordMapper _mapper = null!;
    private MedicalRecordService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IMedicalRecordsRepository>();
        _mapper = new MedicalRecordMapper();
        _service = new MedicalRecordService(_repository, _mapper, NullLogger<MedicalRecordService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidData_ReturnsDetailsDto()
    {
        // Arrange
        var dto = new MedicalRecordCreateDto
        {
            PatientId = 1,
            BloodType = "A+",
            Allergies = ["penicylina"],
            ChronicDiseases = ["cukrzyca"]
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.BloodType, Is.EqualTo("A+"));
        Assert.That(result.PatientId, Is.EqualTo(1));
        await _repository.Received(1).AddAsync(Arg.Is<MedicalRecord>(r =>
            r.PatientId == 1 &&
            r.BloodType == "A+" &&
            r.CreatedAt != default));
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task CreateAsync_SetsCreatedAtToUtcNow()
    {
        // Arrange
        var before = DateTime.UtcNow;
        var dto = new MedicalRecordCreateDto { PatientId = 1, BloodType = "B-" };

        // Act
        await _service.CreateAsync(dto);

        // Assert
        var after = DateTime.UtcNow;
        await _repository.Received(1).AddAsync(Arg.Is<MedicalRecord>(r =>
            r.CreatedAt >= before && r.CreatedAt <= after));
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsDetailsDto()
    {
        // Arrange
        var record = BuildRecord(5, 2, "AB+");
        _repository.GetByIdAsync(5).Returns(record);

        // Act
        var result = await _service.GetByIdAsync(5);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(5));
        Assert.That(result.BloodType, Is.EqualTo("AB+"));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((MedicalRecord?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.That(result, Is.Null);
    }

    // --- GetPagedAsync ---

    [Test]
    public async Task GetPagedAsync_ReturnsListItemDtos()
    {
        // Arrange
        var records = new List<MedicalRecord>
        {
            BuildRecord(1, 1, "0+"),
            BuildRecord(2, 2, "A-")
        };
        _repository.GetPagedAsync(1, 20).Returns(records);

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].BloodType, Is.EqualTo("0+"));
        Assert.That(result[1].BloodType, Is.EqualTo("A-"));
    }

    // --- SearchAsync ---

    [Test]
    public async Task SearchAsync_ValidQuery_DelegatesToRepositoryWithTrimmedQuery()
    {
        // Arrange
        var records = new List<MedicalRecord> { BuildRecord(1, 1, "A+") };
        _repository.SearchAsync("Kowalski").Returns(records);

        // Act
        var result = await _service.SearchAsync("  Kowalski  ");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("Kowalski");
    }

    [Test]
    public async Task SearchAsync_EmptyQuery_ReturnsEmptyListWithoutCallingRepository()
    {
        // Act
        var result = await _service.SearchAsync("   ");

        // Assert
        Assert.That(result, Is.Empty);
        await _repository.DidNotReceive().SearchAsync(Arg.Any<string>());
    }

    [Test]
    public async Task SearchAsync_NullQuery_ReturnsEmptyListWithoutCallingRepository()
    {
        // Act
        var result = await _service.SearchAsync(null!);

        // Assert
        Assert.That(result, Is.Empty);
        await _repository.DidNotReceive().SearchAsync(Arg.Any<string>());
    }

    // --- UpdateAsync ---

    [Test]
    public async Task UpdateAsync_ExistingRecord_UpdatesAndReturnsDto()
    {
        // Arrange
        var record = BuildRecord(3, 1, "B+");
        record.Allergies = ["aspiryna"];
        _repository.GetByIdAsync(3).Returns(record);

        var dto = new MedicalRecordUpdateDto
        {
            Allergies = ["aspiryna", "penicylina"],
            Notes = ["notatka testowa"]
        };

        // Act
        var result = await _service.UpdateAsync(3, dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        await _repository.Received(1).UpdateAsync(record);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void UpdateAsync_NonExistingRecord_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((MedicalRecord?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(99, new MedicalRecordUpdateDto()));
    }

    [Test]
    public async Task UpdateAsync_NullFields_DoesNotOverwriteExistingData()
    {
        // Arrange – UpdateDto z wszystkimi polami null (patch semantics)
        var record = BuildRecord(4, 1, "0-");
        record.Allergies = ["laktozy"];
        record.ChronicDiseases = ["astma"];
        _repository.GetByIdAsync(4).Returns(record);

        var dto = new MedicalRecordUpdateDto(); // wszystkie pola null

        // Act
        await _service.UpdateAsync(4, dto);

        // Assert – mapper nie powinien nadpisać kolekcji gdy dto.Allergies == null
        Assert.That(record.Allergies, Contains.Item("laktozy"));
        Assert.That(record.ChronicDiseases, Contains.Item("astma"));
    }

    // --- DeleteAsync ---

    [Test]
    public async Task DeleteAsync_ExistingRecord_RemovesRecord()
    {
        // Arrange
        var record = BuildRecord(1, 1, "A+");
        _repository.GetByIdAsync(1).Returns(record);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        await _repository.Received(1).DeleteAsync(record);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void DeleteAsync_NonExistingRecord_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((MedicalRecord?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
        _repository.DidNotReceive().DeleteAsync(Arg.Any<MedicalRecord>());
    }

    [Test]
    public async Task DeleteAsync_NonExistingRecord_DoesNotCallSaveChanges()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((MedicalRecord?)null);

        // Act
        try { await _service.DeleteAsync(99); } catch (KeyNotFoundException) { }

        // Assert
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    // --- Helpers ---

    private static MedicalRecord BuildRecord(int id, int patientId, string bloodType) => new()
    {
        Id = id,
        PatientId = patientId,
        BloodType = bloodType,
        Allergies = new List<string>(),
        ChronicDiseases = new List<string>(),
        Notes = new List<string>(),
        MedicalDocuments = new List<MedicalDocument>(),
        CreatedAt = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc),
        Patient = new Patient
        {
            Id = patientId,
            FirstName = "Jan",
            LastName = "Testowy",
            Pesel = "00000000000"
        }
    };
}
