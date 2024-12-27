using System.Text.Json;
using ClinicalTrialsAPI.Application.Commands;
using ClinicalTrialsAPI.Application.Handlers;
using ClinicalTrialsAPI.Domain.Entities;
using ClinicalTrialsAPI.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Xunit;

public class AddClinicalTrialHandlerTests
{
    [Fact]
    public async Task Handle_ValidJson_ShouldAddClinicalTrial()
    {
        // Arrange
        var mockRepository = new Mock<IClinicalTrialRepository>();
        var handler = new AddClinicalTrialHandler(mockRepository.Object);
        var validJson = JsonSerializer.Serialize(new
        {
            trialId = "CT-001",
            title = "COVID-19 Vaccine Study",
            startDate = "2024-01-01",
            endDate = "2024-12-31",
            participants = 150,
            status = "Ongoing"
        });

        var command = new AddClinicalTrialCommand(validJson);

        // Act
        await handler.Handle(command);

        // Assert
        mockRepository.Verify(r => r.AddAsync(It.IsAny<ClinicalTrial>()), Times.Once);
    }

    [Fact]
    public async Task Handle_InvalidJson_ShouldThrowArgumentException()
    {
        // Arrange
        var mockRepository = new Mock<IClinicalTrialRepository>();
        var handler = new AddClinicalTrialHandler(mockRepository.Object);
        var invalidJson = "invalid json";

        var command = new AddClinicalTrialCommand(invalidJson);

        // Act
        var act = async () => await handler.Handle(command);

        // Assert
        await act.Should().ThrowAsync<ArgumentException>().WithMessage("Invalid JSON");
    }

    [Fact]
    public async Task Handle_OngoingStatus_ShouldSetDefaultEndDate()
    {
        // Arrange
        var mockRepository = new Mock<IClinicalTrialRepository>();
        var handler = new AddClinicalTrialHandler(mockRepository.Object);
        var jsonWithOngoingStatus = JsonSerializer.Serialize(new
        {
            trialId = "CT-002",
            title = "Cancer Study",
            startDate = "2024-01-01",
            participants = 200,
            status = "Ongoing"
        });

        var command = new AddClinicalTrialCommand(jsonWithOngoingStatus);

        // Act
        await handler.Handle(command);

        // Assert
        mockRepository.Verify(r => r.AddAsync(It.Is<ClinicalTrial>(trial =>
            trial.EndDate.HasValue &&
            trial.EndDate.Value == DateTime.Parse("2024-02-01") &&
            trial.Duration == 31
        )), Times.Once);
    }
}
