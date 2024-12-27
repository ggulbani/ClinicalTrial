using ClinicalTrialsAPI.Application.Queries;
using ClinicalTrialsAPI.Domain.Entities;
using ClinicalTrialsAPI.Domain.Interfaces;

namespace ClinicalTrialsAPI.Application.Handlers;

public class GetClinicalTrialByIdHandler
{
    private readonly IClinicalTrialRepository _repository;

    public GetClinicalTrialByIdHandler(IClinicalTrialRepository repository)
    {
        _repository = repository;
    }

    public async Task<ClinicalTrial?> Handle(GetClinicalTrialByIdQuery query)
    {
        return await _repository.GetByIdAsync(query.Id);
    }
}
