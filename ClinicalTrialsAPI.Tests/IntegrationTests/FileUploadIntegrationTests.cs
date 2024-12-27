using ClinicalTrialsAPI.Tests;
using System.Net;
using System.Text;
using Xunit;

public class FileUploadIntegrationTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public FileUploadIntegrationTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Upload_ValidJsonFile_ShouldReturn200Ok()
    {
        // Arrange
        var content = new MultipartFormDataContent();
        var validJson = @"{
            ""trialId"": ""CT-003"",
            ""title"": ""Diabetes Study"",
            ""startDate"": ""2024-01-01"",
            ""endDate"": ""2024-12-31"",
            ""participants"": 100,
            ""status"": ""Ongoing""
        }";
        content.Add(new StringContent(validJson, Encoding.UTF8, "application/json"), "file", "test.json");

        // Act
        var response = await _client.PostAsync("/api/clinicaltrials/upload", content);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Upload_InvalidFile_ShouldReturn400BadRequest()
    {
        // Arrange
        var content = new MultipartFormDataContent();
        var invalidJson = "invalid json";
        content.Add(new StringContent(invalidJson, Encoding.UTF8, "application/json"), "file", "test.txt");

        // Act
        var response = await _client.PostAsync("/api/clinicaltrials/upload", content);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
