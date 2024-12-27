using ClinicalTrialsAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialsAPI.Infrastructure.Persistence;

public class ClinicalTrialDbContext : DbContext
{
    public ClinicalTrialDbContext(DbContextOptions options) : base(options) { }

    public DbSet<ClinicalTrial> ClinicalTrials { get; set; } = null!;

    public override int SaveChanges()
    {
        ConvertDateTimesToUtc();
        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ConvertDateTimesToUtc();
        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ConvertDateTimesToUtc()
    {
        foreach (var entry in ChangeTracker.Entries<ClinicalTrial>())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.StartDate = DateTime.SpecifyKind(entry.Entity.StartDate, DateTimeKind.Utc);

                if (entry.Entity.EndDate.HasValue)
                {
                    entry.Entity.EndDate = DateTime.SpecifyKind(entry.Entity.EndDate.Value, DateTimeKind.Utc);
                }
            }
        }
    }
}
