using ClinicalTrialsAPI.Application.Commands;
using ClinicalTrialsAPI.Application.Handlers;
using ClinicalTrialsAPI.Application.Queries;

namespace ClinicalTrialsAPI.Api.Endpoints;

public static class ClinicalTrialEndpoints
{
    public static void MapEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/clinicaltrials").DisableAntiforgery();

        group.MapPost("/upload", async (AddClinicalTrialHandler handler, IFormFile file) =>
        {
            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || extension.ToLower() != ".json")
            {
                return Results.BadRequest("Invalid file type. Only .json files are allowed.");
            }

            // 1 MB limit
            if (file.Length > 1 * 1024 * 1024)
            {
                return Results.BadRequest("File size exceeds the 1 MB limit.");
            }
            
            string json;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                json = await reader.ReadToEndAsync();
            }
            
            await handler.Handle(new AddClinicalTrialCommand(json));
            return Results.Ok("Clinical trial uploaded successfully.");
        })
.WithName("UploadClinicalTrial")
.WithOpenApi(operation =>
{    
    operation.Parameters.Clear(); 
    operation.RequestBody = new Microsoft.OpenApi.Models.OpenApiRequestBody
    {
        Content = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiMediaType>
        {
            ["multipart/form-data"] = new Microsoft.OpenApi.Models.OpenApiMediaType
            {
                Schema = new Microsoft.OpenApi.Models.OpenApiSchema
                {
                    Type = "object",
                    Properties = new Dictionary<string, Microsoft.OpenApi.Models.OpenApiSchema>
                    {
                        ["file"] = new Microsoft.OpenApi.Models.OpenApiSchema
                        {
                            Type = "string",
                            Format = "binary"
                        }
                    },
                    Required = new HashSet<string> { "file" }
                }
            }
        }
    };
    return operation;
});

        group.MapGet("/{id}", async (GetClinicalTrialByIdHandler handler, int id) =>
        {
            var trial = await handler.Handle(new GetClinicalTrialByIdQuery(id));
            return trial != null ? Results.Ok(trial) : Results.NotFound();
        })
        .WithName("GetClinicalTrialById")
        .WithOpenApi();

        group.MapGet("/", async (GetClinicalTrialsByStatusHandler handler, string? status) =>
        {
            var trials = await handler.Handle(new GetClinicalTrialsByStatusQuery(status ?? string.Empty));
            return Results.Ok(trials);
        })
        .WithName("GetClinicalTrialsByStatus")
        .WithOpenApi();
    }
}
