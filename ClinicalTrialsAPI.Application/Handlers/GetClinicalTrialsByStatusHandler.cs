using ClinicalTrialsAPI.Application.Queries;
using ClinicalTrialsAPI.Domain.Entities;
using ClinicalTrialsAPI.Domain.Interfaces;

namespace ClinicalTrialsAPI.Application.Handlers;

public class GetClinicalTrialsByStatusHandler
{
    private readonly IClinicalTrialRepository _repository;

    public GetClinicalTrialsByStatusHandler(IClinicalTrialRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ClinicalTrial>> Handle(GetClinicalTrialsByStatusQuery query)
    {
        return await _repository.GetByStatusAsync(query.Status);
    }
}
