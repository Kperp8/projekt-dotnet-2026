using Application.Patients.Dtos;
using Application.Patients.Mappers;
using Application.Patients.Services;
using Domain.Patients;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.Patients;

[TestFixture]
public class PatientServiceTests
{
    private IPatientRepository _repository = null!;
    private PatientMapper _mapper = null!;
    private PatientService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IPatientRepository>();
        _mapper = new PatientMapper();
        _service = new PatientService(_repository, _mapper, NullLogger<PatientService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidData_ReturnsPatientDetailsDto()
    {
        // Arrange
        var dto = new PatientCreateDto
        {
            Pesel = "12345678901",
            FirstName = "Jan",
            LastName = "Kowalski",
            BirthDate = new DateTime(1990, 1, 1),
            Email = "jan@example.com"
        };
        _repository.ExistsByPeselAsync(dto.Pesel).Returns(false);

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.Pesel, Is.EqualTo(dto.Pesel));
        Assert.That(result.FirstName, Is.EqualTo(dto.FirstName));
        Assert.That(result.LastName, Is.EqualTo(dto.LastName));
        await _repository.Received(1).AddAsync(Arg.Any<Patient>());
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task CreateAsync_DuplicatePesel_ThrowsInvalidOperationException()
    {
        // Arrange
        var dto = new PatientCreateDto { Pesel = "12345678901", FirstName = "Jan", LastName = "Kowalski", BirthDate = DateTime.Today };
        _repository.ExistsByPeselAsync(dto.Pesel).Returns(true);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _service.CreateAsync(dto));
        await _repository.DidNotReceive().AddAsync(Arg.Any<Patient>());
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsPatientDetailsDto()
    {
        // Arrange
        var patient = BuildPatient(1, "12345678901", "Anna", "Nowak");
        _repository.GetByIdAsync(1).Returns(patient);

        // Act
        var result = await _service.GetByIdAsync(1);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(1));
        Assert.That(result.LastName, Is.EqualTo("Nowak"));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Patient?)null);

        // Act
        var result = await _service.GetByIdAsync(99);

        // Assert – kontroler powinien zamienić null na 404
        Assert.That(result, Is.Null);
    }

    // --- SearchAsync ---

    [Test]
    public async Task SearchAsync_ByLastNameFragment_ReturnsMatchingPatients()
    {
        // Arrange
        var patients = new List<Patient>
        {
            BuildPatient(1, "12345678901", "Jan", "Kowalski"),
            BuildPatient(2, "98765432101", "Anna", "Kowalczyk")
        };
        _repository.SearchAsync("Kowal").Returns(patients);

        // Act
        var result = await _service.SearchAsync("Kowal");

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].LastName, Does.StartWith("Kowal"));
    }

    [Test]
    public async Task SearchAsync_ByExactPesel_ReturnsSinglePatient()
    {
        // Arrange
        var patients = new List<Patient> { BuildPatient(1, "12345678901", "Jan", "Kowalski") };
        _repository.SearchAsync("12345678901").Returns(patients);

        // Act
        var result = await _service.SearchAsync("12345678901");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        Assert.That(result[0].Pesel, Is.EqualTo("12345678901"));
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
    public async Task UpdateAsync_ExistingPatient_UpdatesAndReturnsDto()
    {
        // Arrange
        var patient = BuildPatient(1, "12345678901", "Jan", "Kowalski");
        _repository.GetByIdAsync(1).Returns(patient);
        var dto = new PatientUpdateDto { FirstName = "Janusz", LastName = "Kowalski", BirthDate = DateTime.Today };

        // Act
        var result = await _service.UpdateAsync(1, dto);

        // Assert
        Assert.That(result.FirstName, Is.EqualTo("Janusz"));
        await _repository.Received(1).UpdateAsync(patient);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void UpdateAsync_NonExistingPatient_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Patient?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(99, new PatientUpdateDto { FirstName = "X", LastName = "Y", BirthDate = DateTime.Today }));
    }

    // --- DeleteAsync ---

    [Test]
    public async Task DeleteAsync_ExistingPatient_SetsIsDeletedTrue()
    {
        // Arrange
        var patient = BuildPatient(1, "12345678901", "Jan", "Kowalski");
        _repository.GetByIdAsync(1).Returns(patient);

        // Act
        await _service.DeleteAsync(1);

        // Assert – soft delete, nie fizyczne usunięcie
        Assert.That(patient.IsDeleted, Is.True);
        await _repository.Received(1).UpdateAsync(patient);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void DeleteAsync_NonExistingPatient_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Patient?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
    }

    // --- Helpers ---

    private static Patient BuildPatient(int id, string pesel, string firstName, string lastName) => new()
    {
        Id = id,
        Pesel = pesel,
        FirstName = firstName,
        LastName = lastName,
        BirthDate = new DateTime(1990, 1, 1),
        IsDeleted = false
    };
}
