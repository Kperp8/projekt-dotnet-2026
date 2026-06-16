using Application.Medications.Dtos;
using Application.Medications.Mappers;
using Application.Medications.Services;
using Domain.Medications;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.Medications;

[TestFixture]
public class MedicationServiceTests
{
    private IMedicationsRepository _repository = null!;
    private MedicationMapper _mapper = null!;
    private MedicationService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IMedicationsRepository>();
        _mapper = new MedicationMapper();
        _service = new MedicationService(_repository, _mapper, NullLogger<MedicationService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidData_ReturnsMedicationDetailsDto()
    {
        // Arrange
        var dto = new MedicationCreateDto
        {
            Name = "Ibuprofen",
            Dosing = "3x dziennie 400 mg",
            Cost = 12.50m
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Ibuprofen"));
        Assert.That(result.Dosing, Is.EqualTo("3x dziennie 400 mg"));
        Assert.That(result.Cost, Is.EqualTo(12.50m));
        await _repository.Received(1).AddAsync(Arg.Any<Medication>());
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task CreateAsync_SetsQuantityToDefault()
    {
        // Arrange
        var dto = new MedicationCreateDto { Name = "Paracetamol", Dosing = "1x 500 mg", Cost = 5.00m };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert – Quantity nie pochodzi z DTO, powinno mieć wartość domyślną (0)
        Assert.That(result.Quantity, Is.EqualTo(0));
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsMedicationDetailsDto()
    {
        // Arrange
        var medication = BuildMedication(1, "Aspiryna", "1x 100 mg", 8.00m);
        _repository.GetByIdAsync(1).Returns(medication);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Name, Is.EqualTo("Aspiryna"));
        Assert.That(result.Dosing, Is.EqualTo("1x 100 mg"));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Medication?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.That(result, Is.Null);
    }

    // --- GetPagedAsync ---

    [Test]
    public async Task GetPagedAsync_ReturnsMappedListItems()
    {
        // Arrange
        var medications = new List<Medication>
        {
            BuildMedication(1, "Ibuprofen", "3x 400 mg", 12.50m),
            BuildMedication(2, "Paracetamol", "1x 500 mg", 5.00m)
        };
        _repository.GetPagedAsync(1, 20).Returns(medications);

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Name, Is.EqualTo("Ibuprofen"));
        Assert.That(result[1].Name, Is.EqualTo("Paracetamol"));
    }

    [Test]
    public async Task GetPagedAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetPagedAsync(1, 20).Returns(new List<Medication>());

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result, Is.Empty);
    }

    // --- SearchAsync ---

    [Test]
    public async Task SearchAsync_MatchingQuery_ReturnsMatchingMedications()
    {
        // Arrange
        var medications = new List<Medication>
        {
            BuildMedication(1, "Ibuprofen", "3x 400 mg", 12.50m),
            BuildMedication(2, "Ibumax", "2x 200 mg", 9.00m)
        };
        _repository.SearchAsync("Ibu").Returns(medications);

        // Act
        var result = await _service.SearchAsync("Ibu");

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        await _repository.Received(1).SearchAsync("Ibu");
    }

    [Test]
    public async Task SearchAsync_WhitespaceQuery_ReturnsEmptyListWithoutCallingRepository()
    {
        // Act
        var result = await _service.SearchAsync("   ");

        // Assert
        Assert.That(result, Is.Empty);
        await _repository.DidNotReceive().SearchAsync(Arg.Any<string>());
    }

    [Test]
    public async Task SearchAsync_EmptyString_ReturnsEmptyListWithoutCallingRepository()
    {
        // Act
        var result = await _service.SearchAsync(string.Empty);

        // Assert
        Assert.That(result, Is.Empty);
        await _repository.DidNotReceive().SearchAsync(Arg.Any<string>());
    }

    [Test]
    public async Task SearchAsync_TrimsQueryBeforePassingToRepository()
    {
        // Arrange
        _repository.SearchAsync("Aspiryna").Returns(new List<Medication>());

        // Act
        await _service.SearchAsync("  Aspiryna  ");

        // Assert – przekazuje przycięty ciąg
        await _repository.Received(1).SearchAsync("Aspiryna");
    }

    // --- UpdateAsync ---

    [Test]
    public async Task UpdateAsync_ExistingMedication_UpdatesDosingAndCost()
    {
        // Arrange
        var medication = BuildMedication(1, "Ibuprofen", "3x 400 mg", 12.50m);
        _repository.GetByIdAsync(1).Returns(medication);
        var dto = new MedicationUpdateDto { Dosing = "2x 400 mg", Cost = 14.00m };

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.That(result.Dosing, Is.EqualTo("2x 400 mg"));
        Assert.That(result.Cost, Is.EqualTo(14.00m));
        await _repository.Received(1).UpdateAsync(medication);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_NullDosing_DoesNotOverwriteExistingDosing()
    {
        // Arrange
        var medication = BuildMedication(1, "Ibuprofen", "3x 400 mg", 12.50m);
        _repository.GetByIdAsync(1).Returns(medication);
        var dto = new MedicationUpdateDto { Dosing = null, Cost = 15.00m };

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert – dozowanie nienaruszone
        Assert.That(result.Dosing, Is.EqualTo("3x 400 mg"));
        Assert.That(result.Cost, Is.EqualTo(15.00m));
    }

    [Test]
    public void UpdateAsync_NonExistingMedication_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Medication?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(99, new MedicationUpdateDto { Dosing = "1x 200 mg", Cost = 10.00m }));
    }

    [Test]
    public async Task UpdateAsync_NonExistingMedication_DoesNotCallRepositoryUpdate()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Medication?)null);

        // Act
        try { await _service.UpdateAsync(99, new MedicationUpdateDto { Cost = 5.00m }); }
        catch (KeyNotFoundException) { }

        // Assert
        await _repository.DidNotReceive().UpdateAsync(Arg.Any<Medication>());
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    // --- DeleteAsync ---

    [Test]
    public async Task DeleteAsync_ExistingMedication_CallsDeleteAndSave()
    {
        // Arrange
        var medication = BuildMedication(1, "Aspiryna", "1x 100 mg", 8.00m);
        _repository.GetByIdAsync(1).Returns(medication);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        await _repository.Received(1).DeleteAsync(medication);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void DeleteAsync_NonExistingMedication_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Medication?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
    }

    [Test]
    public async Task DeleteAsync_NonExistingMedication_DoesNotCallRepositoryDelete()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Medication?)null);

        // Act
        try { await _service.DeleteAsync(99); }
        catch (KeyNotFoundException) { }

        // Assert
        await _repository.DidNotReceive().DeleteAsync(Arg.Any<Medication>());
        await _repository.DidNotReceive().SaveChangesAsync();
    }

    // --- Helpers ---

    private static Medication BuildMedication(int id, string name, string dosing, decimal cost, int quantity = 0) => new()
    {
        Id = id,
        Name = name,
        Dosing = dosing,
        Cost = cost,
        Quantity = quantity
    };
}
