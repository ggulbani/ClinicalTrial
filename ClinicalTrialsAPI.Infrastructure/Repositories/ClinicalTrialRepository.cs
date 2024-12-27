using ClinicalTrialsAPI.Domain.Entities;
using ClinicalTrialsAPI.Domain.Interfaces;
using ClinicalTrialsAPI.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialsAPI.Infrastructure.Repositories;

public class ClinicalTrialRepository : IClinicalTrialRepository
{
    private readonly ClinicalTrialDbContext _context;

    public ClinicalTrialRepository(ClinicalTrialDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(ClinicalTrial trial)
    {
        _context.ClinicalTrials.Add(trial);
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            var asdfas = ex.InnerException;
            throw;
        }
        
    }

    public async Task<ClinicalTrial?> GetByIdAsync(int id)
    {
        return await _context.ClinicalTrials.FindAsync(id);
    }

    public async Task<IEnumerable<ClinicalTrial>> GetByStatusAsync(string status)
    {
        return await _context.ClinicalTrials.Where(t => t.Status == status).ToListAsync();
    }
}
