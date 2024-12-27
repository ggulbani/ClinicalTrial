using ClinicalTrialsAPI.Application.Commands;
using ClinicalTrialsAPI.Domain.Entities;
using ClinicalTrialsAPI.Domain.Interfaces;
using System.Text.Json;

namespace ClinicalTrialsAPI.Application.Handlers;

public class AddClinicalTrialHandler
{
    private readonly IClinicalTrialRepository _repository;

    public AddClinicalTrialHandler(IClinicalTrialRepository repository)
    {
        _repository = repository;
    }

    public async Task Handle(AddClinicalTrialCommand command)
    {
        try
        {
            var data = JsonSerializer.Deserialize<ClinicalTrial>(command.Json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (data == null)
                throw new ArgumentException("Invalid JSON");

            if (data.Status == "Ongoing" && !data.EndDate.HasValue)
                data.EndDate = data.StartDate.AddMonths(1);

            data.Duration = (data.EndDate - data.StartDate)?.Days ?? 0;

            await _repository.AddAsync(data);
        }
        catch (JsonException ex)
        {
            throw new ArgumentException("Invalid JSON", ex);
        }
    }
}
