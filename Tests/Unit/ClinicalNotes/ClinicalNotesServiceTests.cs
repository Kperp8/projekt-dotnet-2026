using Application.ClinicalNotes.Dtos;
using Application.ClinicalNotes.Mappers;
using Application.ClinicalNotes.Services;
using Domain.ClinicalNotes;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.ClinicalNotes;

[TestFixture]
public class ClinicalNotesServiceTests
{
    private IClinicalNotesRepository _repository = null!;
    private ClinicalNotesMapper _mapper = null!;
    private ClinicalNotesService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IClinicalNotesRepository>();
        _mapper = new ClinicalNotesMapper();
        _service = new ClinicalNotesService(_repository, _mapper, NullLogger<ClinicalNotesService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidDto_AddsEntityAndReturnsDetailsDto()
    {
        // Arrange
        var dto = new ClinicalNotesCreateDto
        {
            MedicalHistory  = "Pacjent zgłasza ból głowy od 3 dni.",
            Diagnosis       = "Migrena",
            Recommendations = "Odpoczynek, leki przeciwbólowe"
        };

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.MedicalHistory, Is.EqualTo("Pacjent zgłasza ból głowy od 3 dni."));
        Assert.That(result.Diagnosis, Is.EqualTo("Migrena"));
        Assert.That(result.Recommendations, Is.EqualTo("Odpoczynek, leki przeciwbólowe"));
        await _repository.Received(1).AddAsync(Arg.Any<ClinicalNote>());
        await _repository.Received(1).SaveChangesAsync();
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsDetailsDto()
    {
        // Arrange
        var note = BuildNote(1, "Wywiad", "Grypa", "Leczenie objawowe");
        _repository.GetByIdAsync(1).Returns(note);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.Diagnosis, Is.EqualTo("Grypa"));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((ClinicalNote?)null);

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
        var notes = new List<ClinicalNote>
        {
            BuildNote(1, "Wywiad A", "Diagnoza A", "Zalecenia A"),
            BuildNote(2, "Wywiad B", "Diagnoza B", "Zalecenia B")
        };
        _repository.GetPagedAsync(1, 20).Returns(notes);

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Diagnosis, Is.EqualTo("Diagnoza A"));
        Assert.That(result[1].Diagnosis, Is.EqualTo("Diagnoza B"));
    }

    [Test]
    public async Task GetPagedAsync_EmptyRepository_ReturnsEmptyList()
    {
        // Arrange
        _repository.GetPagedAsync(1, 20).Returns(new List<ClinicalNote>());

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
        var notes = new List<ClinicalNote> { BuildNote(1, "Wywiad", "Migrena", "Odpoczynek") };
        _repository.SearchAsync("Migrena").Returns(notes);

        // Act
        var result = await _service.SearchAsync("Migrena");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("Migrena");
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
    public async Task SearchAsync_QueryWithSurroundingWhitespace_TrimsBeforePassingToRepository()
    {
        // Arrange
        _repository.SearchAsync("Grypa").Returns(new List<ClinicalNote>());

        // Act
        await _service.SearchAsync("  Grypa  ");

        // Assert
        await _repository.Received(1).SearchAsync("Grypa");
    }

    // --- UpdateAsync ---

    [Test]
    public async Task UpdateAsync_ExistingId_UpdatesOnlyProvidedFields()
    {
        // Arrange
        var note = BuildNote(3, "Stary wywiad", "Stara diagnoza", "Stare zalecenia");
        _repository.GetByIdAsync(3).Returns(note);

        var dto = new ClinicalNotesUpdateDto { Diagnosis = "Nowa diagnoza" }; // tylko diagnoza

        // Act
        var result = await _service.UpdateAsync(3, dto);

        // Assert
        Assert.That(note.MedicalHistory, Is.EqualTo("Stary wywiad"));      // nienaruszone
        Assert.That(note.Diagnosis, Is.EqualTo("Nowa diagnoza"));          // zmienione
        Assert.That(note.Recommendations, Is.EqualTo("Stare zalecenia"));  // nienaruszone
        await _repository.Received(1).UpdateAsync(note);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task UpdateAsync_ExistingId_UpdatesAllProvidedFields()
    {
        // Arrange
        var note = BuildNote(4, "Stary wywiad", "Stara diagnoza", "Stare zalecenia");
        _repository.GetByIdAsync(4).Returns(note);

        var dto = new ClinicalNotesUpdateDto
        {
            MedicalHistory  = "Nowy wywiad",
            Diagnosis       = "Nowa diagnoza",
            Recommendations = "Nowe zalecenia"
        };

        // Act
        await _service.UpdateAsync(4, dto);

        // Assert
        Assert.That(note.MedicalHistory, Is.EqualTo("Nowy wywiad"));
        Assert.That(note.Diagnosis, Is.EqualTo("Nowa diagnoza"));
        Assert.That(note.Recommendations, Is.EqualTo("Nowe zalecenia"));
    }

    [Test]
    public async Task UpdateAsync_ExistingId_ReturnsUpdatedDetailsDto()
    {
        // Arrange
        var note = BuildNote(5, "Wywiad", "Stara diagnoza", "Zalecenia");
        _repository.GetByIdAsync(5).Returns(note);

        var dto = new ClinicalNotesUpdateDto { Diagnosis = "Zaktualizowana diagnoza" };

        // Act
        var result = await _service.UpdateAsync(5, dto);

        // Assert
        Assert.That(result.Diagnosis, Is.EqualTo("Zaktualizowana diagnoza"));
    }

    [Test]
    public void UpdateAsync_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((ClinicalNote?)null);

        var dto = new ClinicalNotesUpdateDto { Diagnosis = "X" };

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.UpdateAsync(99, dto));
        _repository.DidNotReceive().UpdateAsync(Arg.Any<ClinicalNote>());
    }

    // --- DeleteAsync ---

    [Test]
    public async Task DeleteAsync_ExistingId_RemovesNote()
    {
        // Arrange
        var note = BuildNote(6, "Wywiad", "Diagnoza", "Zalecenia");
        _repository.GetByIdAsync(6).Returns(note);

        // Act
        await _service.DeleteAsync(6);

        // Assert
        await _repository.Received(1).DeleteAsync(note);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void DeleteAsync_NonExistingId_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((ClinicalNote?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
        _repository.DidNotReceive().DeleteAsync(Arg.Any<ClinicalNote>());
    }

    // --- helper ---

    private static ClinicalNote BuildNote(int id, string medicalHistory, string diagnosis, string recommendations)
        => new()
        {
            Id              = id,
            MedicalHistory  = medicalHistory,
            Diagnosis       = diagnosis,
            Recommendations = recommendations
        };
}
