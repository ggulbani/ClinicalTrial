using ClinicalTrialsAPI.Domain.Entities;

namespace ClinicalTrialsAPI.Domain.Interfaces
{
    public interface IClinicalTrialRepository
    {
        Task AddAsync(ClinicalTrial trial);
        Task<ClinicalTrial?> GetByIdAsync(int id);
        Task<IEnumerable<ClinicalTrial>> GetByStatusAsync(string status);
    }
}
