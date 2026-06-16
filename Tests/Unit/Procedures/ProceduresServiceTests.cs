using Application.Procedures.Dtos;
using Application.Procedures.Mappers;
using Application.Procedures.Services;
using Domain.Procedures;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.Procedures;

[TestFixture]
public class ProceduresServiceTests
{
    private IProceduresRepository _repository = null!;
    private ProceduresMapper _mapper = null!;
    private ProceduresService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IProceduresRepository>();
        _mapper = new ProceduresMapper();
        _service = new ProceduresService(_repository, _mapper, NullLogger<ProceduresService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidDto_AddsEntityAndReturnsDetailsDto()
    {
        // Arrange
        var dto = new ProceduresCreateDto
        {
            Name        = "Morfologia krwi",
            Description = "Podstawowe badanie krwi",
            Price       = 35.00m
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Name, Is.EqualTo("Morfologia krwi"));
        Assert.That(result.Price, Is.EqualTo(35.00m));
        await _repository.Received(1).AddAsync(Arg.Any<Procedure>());
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task CreateAsync_ValidDto_ReturnedDtoHasCorrectDescription()
    {
        // Arrange
        var dto = new ProceduresCreateDto
        {
            Name        = "EKG",
            Description = "Badanie elektrokardiograficzne",
            Price       = 60.00m
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result.Description, Is.EqualTo("Badanie elektrokardiograficzne"));
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsDetailsDto()
    {
        // Arrange
        var procedure = BuildProcedure(5, "RTG klatki piersiowej", "Zdjęcie rentgenowskie", 80.00m);
        _repository.GetByIdAsync(5).Returns(procedure);

        // Act
        var result = await _service.GetByIdAsync(5);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(5));
        Assert.That(result.Name, Is.EqualTo("RTG klatki piersiowej"));
        Assert.That(result.Price, Is.EqualTo(80.00m));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Procedure?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert
        Assert.That(result, Is.Null);
    }

    // --- GetPagedAsync ---

    [Test]
    public async Task GetPagedAsync_ReturnsCorrectPage()
    {
        // Arrange
        var procedures = new List<Procedure>
        {
            BuildProcedure(1, "Morfologia", "Opis", 35.00m),
            BuildProcedure(2, "EKG",        "Opis", 60.00m)
        };
        _repository.GetPagedAsync(1, 20).Returns(procedures);

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(1));
        Assert.That(result[1].Name, Is.EqualTo("EKG"));
    }

    [Test]
    public async Task GetPagedAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetPagedAsync(1, 20).Returns(new List<Procedure>());

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result, Is.Empty);
    }

    // --- SearchAsync ---

    [Test]
    public async Task SearchAsync_NonEmptyQuery_DelegatesToRepository()
    {
        // Arrange
        var procedures = new List<Procedure> { BuildProcedure(1, "Morfologia", "Opis", 35.00m) };
        _repository.SearchAsync("Morfologia").Returns(procedures);

        // Act
        var result = await _service.SearchAsync("Morfologia");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("Morfologia");
    }

    [Test]
    public async Task SearchAsync_QueryWithWhitespace_TrimsBeforePassingToRepository()
    {
        // Arrange
        var procedures = new List<Procedure> { BuildProcedure(1, "EKG", "Opis", 60.00m) };
        _repository.SearchAsync("EKG").Returns(procedures);

        // Act
        var result = await _service.SearchAsync("  EKG  ");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("EKG");
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

    // --- UpdateAsync ---

    [Test]
    public async Task UpdateAsync_ExistingId_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var procedure = BuildProcedure(3, "Stara nazwa", "Stary opis", 50.00m);
        _repository.GetByIdAsync(3).Returns(procedure);

        var dto = new ProceduresUpdateDto { Price = 75.00m }; // tylko cena

        // Act
        var result = await _service.UpdateAsync(3, dto);

        // Assert
        Assert.That(procedure.Name, Is.EqualTo("Stara nazwa"));    // nie zmieniona
        Assert.That(procedure.Description, Is.EqualTo("Stary opis")); // nie zmieniona
        Assert.That(procedure.Price, Is.EqualTo(75.00m));           // zmieniona
        await _repository.Received(1).UpdateAsync(procedure);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_ExistingId_UpdatesAllProvidedFields()
    {
        // Arrange
        var procedure = BuildProcedure(4, "Stara nazwa", "Stary opis", 50.00m);
        _repository.GetByIdAsync(4).Returns(procedure);

        var dto = new ProceduresUpdateDto
        {
            Name        = "Nowa nazwa",
            Description = "Nowy opis",
            Price       = 99.99m
        };

        // Act
        await _service.UpdateAsync(4, dto);

        // Assert
        Assert.That(procedure.Name, Is.EqualTo("Nowa nazwa"));
        Assert.That(procedure.Description, Is.EqualTo("Nowy opis"));
        Assert.That(procedure.Price, Is.EqualTo(99.99m));
    }

    [Test]
    public async Task UpdateAsync_ExistingId_ReturnsUpdatedDetailsDto()
    {
        // Arrange
        var procedure = BuildProcedure(5, "Morfologia", "Opis", 35.00m);
        _repository.GetByIdAsync(5).Returns(procedure);

        var dto = new ProceduresUpdateDto { Name = "Morfologia rozszerzona" };

        // Act
        var result = await _service.UpdateAsync(5, dto);

        // Assert
        Assert.That(result.Name, Is.EqualTo("Morfologia rozszerzona"));
    }

    [Test]
    public void UpdateAsync_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Procedure?)null);

        var dto = new ProceduresUpdateDto { Price = 50.00m };

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99, dto));
        _repository.DidNotReceive().UpdateAsync(Arg.Any<Procedure>());
    }

    // --- DeleteAsync ---

    [Test]
    public async Task DeleteAsync_ExistingId_RemovesProcedure()
    {
        // Arrange
        var procedure = BuildProcedure(6, "USG jamy brzusznej", "Opis", 120.00m);
        _repository.GetByIdAsync(6).Returns(procedure);

        // Act
        await _service.DeleteAsync(6);

        // Assert
        await _repository.Received(1).DeleteAsync(procedure);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Procedure?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
        _repository.DidNotReceive().DeleteAsync(Arg.Any<Procedure>());
    }

    // --- helper ---

    private static Procedure BuildProcedure(int id, string name, string description, decimal price)
        => new()
        {
            Id          = id,
            Name        = name,
            Description = description,
            Price       = price
        };
}
