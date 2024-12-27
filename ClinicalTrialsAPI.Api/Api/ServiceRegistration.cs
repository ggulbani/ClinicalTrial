using ClinicalTrialsAPI.Application.Handlers;
using ClinicalTrialsAPI.Domain.Interfaces;
using ClinicalTrialsAPI.Infrastructure.Persistence;
using ClinicalTrialsAPI.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ClinicalTrialsAPI.Api;

public static class ServiceRegistration
{
    public static void AddApplicationServices(this IServiceCollection services)
    {        
        services.AddScoped<AddClinicalTrialHandler>();
        services.AddScoped<GetClinicalTrialByIdHandler>();
        services.AddScoped<GetClinicalTrialsByStatusHandler>();   
    }
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ClinicalTrialDbContext>(options =>
        options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));        
        services.AddScoped<IClinicalTrialRepository, ClinicalTrialRepository>();        
    }
}
