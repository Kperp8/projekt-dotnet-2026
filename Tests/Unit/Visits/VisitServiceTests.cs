using Application.Visits.Dtos;
using Application.Visits.Mappers;
using Application.Visits.Services;
using Domain.Visits;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace Tests.Unit.Visits;

[TestFixture]
public class VisitServiceTests
{
    private IVisitsRepository _repository = null!;
    private VisitMapper _mapper = null!;
    private VisitService _service = null!;

    [SetUp]
    public void SetUp()
    {
        _repository = Substitute.For<IVisitsRepository>();
        _mapper = new VisitMapper();
        _service = new VisitService(_repository, _mapper, NullLogger<VisitService>.Instance);
    }

    // --- CreateAsync ---

    [Test]
    public async Task CreateAsync_ValidDto_AddsEntityAndReturnsDetailsDto()
    {
        // Arrange
        var dto = new VisitCreateDto
        {
            PatientId    = 1,
            ScheduledAt  = new DateTime(2026, 7, 1, 10, 0, 0),
            AssignedDoctorId = "doc-1",
            Status       = VisitStatus.Planned
        };

        var savedVisit = BuildVisit(42, 1, VisitStatus.Planned, dto.ScheduledAt, dto.AssignedDoctorId);
        _repository.GetByIdAsync(0).Returns(savedVisit); // GetByIdAsync(visit.Id) po SaveChanges – Id=0 przed bazą

        // Act
        var result = await _service.CreateAsync(dto);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result.PatientId, Is.EqualTo(1));
        Assert.That(result.Status, Is.EqualTo(VisitStatus.Planned));
        await _repository.Received(1).AddAsync(Arg.Any<Visit>());
        await _repository.Received(1).SaveChangesAsync();
    }

    // --- GetByIdAsync ---

    [Test]
    public async Task GetByIdAsync_ExistingId_ReturnsDetailsDto()
    {
        // Arrange
        var visit = BuildVisit(5, 2, VisitStatus.Planned, DateTime.Today, null);
        _repository.GetByIdAsync(5).Returns(visit);

        // Act
        var result = await _service.GetByIdAsync(5);

        // Assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result!.Id, Is.EqualTo(5));
        Assert.That(result.PatientId, Is.EqualTo(2));
    }

    [Test]
    public async Task GetByIdAsync_NonExistingId_ReturnsNull()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Visit?)null);

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
        var visits = new List<Visit>
        {
            BuildVisit(1, 1, VisitStatus.Planned,    DateTime.Today, null),
            BuildVisit(2, 2, VisitStatus.Completed,  DateTime.Today.AddDays(-1), "doc-1")
        };
        _repository.GetPagedAsync(1, 20).Returns(visits);

        // Act
        var result = await _service.GetPagedAsync(1, 20);

        // Assert
        Assert.That(result.Count, Is.EqualTo(2));
        Assert.That(result[0].Id, Is.EqualTo(1));
        Assert.That(result[1].Status, Is.EqualTo(VisitStatus.Completed));
    }

    // --- SearchAsync ---

    [Test]
    public async Task SearchAsync_NonEmptyQuery_DelegatesToRepository()
    {
        // Arrange
        var visits = new List<Visit> { BuildVisit(1, 1, VisitStatus.Planned, DateTime.Today, null) };
        _repository.SearchAsync("Kowalski").Returns(visits);

        // Act
        var result = await _service.SearchAsync("Kowalski");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("Kowalski");
    }

    [Test]
    public async Task SearchAsync_QueryWithWhitespace_TrimsBeforePassingToRepository()
    {
        // Arrange
        var visits = new List<Visit> { BuildVisit(1, 1, VisitStatus.Planned, DateTime.Today, null) };
        _repository.SearchAsync("Nowak").Returns(visits);

        // Act
        var result = await _service.SearchAsync("  Nowak  ");

        // Assert
        Assert.That(result.Count, Is.EqualTo(1));
        await _repository.Received(1).SearchAsync("Nowak");
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
    public async Task UpdateAsync_ExistingVisit_UpdatesAndReturnsDto()
    {
        // Arrange
        var visit = BuildVisit(3, 1, VisitStatus.Planned, new DateTime(2026, 7, 1, 10, 0, 0), null);
        _repository.GetByIdAsync(3).Returns(visit);

        var dto = new VisitUpdateDto
        {
            ScheduledAt      = new DateTime(2026, 7, 2, 12, 0, 0),
            Status           = VisitStatus.InProgress,
            AssignedDoctorId = "doc-2"
        };

        // Act
        var result = await _service.UpdateAsync(3, dto);

        // Assert
        Assert.That(result.Status, Is.EqualTo(VisitStatus.InProgress));
        Assert.That(result.AssignedDoctorId, Is.EqualTo("doc-2"));
        Assert.That(result.ScheduledAt, Is.EqualTo(new DateTime(2026, 7, 2, 12, 0, 0)));
        await _repository.Received(1).UpdateAsync(visit);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public void UpdateAsync_NonExistingVisit_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Visit?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() =>
            _service.UpdateAsync(99, new VisitUpdateDto
            {
                ScheduledAt = DateTime.Today,
                Status      = VisitStatus.Planned
            }));
    }

    // --- DeleteAsync (anulowanie) ---

    [Test]
    public async Task DeleteAsync_PlannedVisit_SetsCancelledStatus()
    {
        // Arrange
        var visit = BuildVisit(7, 1, VisitStatus.Planned, DateTime.Today, null);
        _repository.GetByIdAsync(7).Returns(visit);

        // Act
        await _service.DeleteAsync(7);

        // Assert
        Assert.That(visit.Status, Is.EqualTo(VisitStatus.Cancelled));
        await _repository.Received(1).UpdateAsync(visit);
        await _repository.Received(1).SaveChangesAsync();
    }

    [Test]
    public async Task DeleteAsync_InProgressVisit_SetsCancelledStatus()
    {
        // Arrange
        var visit = BuildVisit(8, 1, VisitStatus.InProgress, DateTime.Today, null);
        _repository.GetByIdAsync(8).Returns(visit);

        // Act
        await _service.DeleteAsync(8);

        // Assert
        Assert.That(visit.Status, Is.EqualTo(VisitStatus.Cancelled));
    }

    [Test]
    public void DeleteAsync_CompletedVisit_ThrowsInvalidOperationException()
    {
        // Arrange
        var visit = BuildVisit(9, 1, VisitStatus.Completed, DateTime.Today, null);
        _repository.GetByIdAsync(9).Returns(visit);

        // Act & Assert
        Assert.ThrowsAsync<InvalidOperationException>(() => _service.DeleteAsync(9));
        _repository.DidNotReceive().UpdateAsync(Arg.Any<Visit>());
    }

    [Test]
    public void DeleteAsync_NonExistingVisit_ThrowsKeyNotFoundException()
    {
        // Arrange
        _repository.GetByIdAsync(99).Returns((Visit?)null);

        // Act & Assert
        Assert.ThrowsAsync<KeyNotFoundException>(() => _service.DeleteAsync(99));
    }

    [Test]
    public async Task DeleteAsync_AlreadyCancelledVisit_SetsCancelledAgainWithoutError()
    {
        // Arrange – anulowanie już anulowanej wizyty nie jest zabronione przez serwis
        var visit = BuildVisit(10, 1, VisitStatus.Cancelled, DateTime.Today, null);
        _repository.GetByIdAsync(10).Returns(visit);

        // Act
        await _service.DeleteAsync(10);

        // Assert
        Assert.That(visit.Status, Is.EqualTo(VisitStatus.Cancelled));
        await _repository.Received(1).UpdateAsync(visit);
    }

    // --- helper ---

    private static Visit BuildVisit(int id, int patientId, VisitStatus status, DateTime scheduledAt, string? doctorId)
    {
        var patient = new Patient
        {
            Id        = patientId,
            FirstName = "Jan",
            LastName  = "Testowy",
            Pesel     = "00000000000"
        };

        return new Visit
        {
            Id               = id,
            PatientId        = patientId,
            Patient          = patient,
            Status           = status,
            ScheduledAt      = scheduledAt,
            AssignedDoctorId = doctorId
        };
    }
}
