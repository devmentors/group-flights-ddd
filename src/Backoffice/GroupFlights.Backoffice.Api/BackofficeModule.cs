using GroupFlights.Backoffice.Core;
using GroupFlights.Backoffice.Core.DTO;
using GroupFlights.Backoffice.Core.Services;
using GroupFlights.Shared.ModuleDefinition;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroupFlights.Backoffice.Api;

public class BackofficeModule : ModuleDefinition
{
    public override string ModulePrefix => "/backoffice";
    
    public override void AddDependencies(IServiceCollection services, IConfiguration configuration)
    {
        services.AddCore(configuration);
    }

    public override IReadOnlyCollection<EndpointRegistration> CreateEndpoints(IEndpointRouteBuilder routeBuilder)
    {
        return new[]
        {
            new EndpointRegistration(
                "contracts/{contractId}",
                HttpVerb.GET,
                NaiveAccessControl.ClientOnly,
                async (
                    [FromServices] IDocumentService documentService,
                    [FromRoute] Guid contractId,
                    CancellationToken cancellationToken) =>
                {
                    var file = await documentService.DownloadContractFile(contractId, cancellationToken);
                    return Results.File(file.Content, "application/octet-stream", file.FileName);
                }),
            new EndpointRegistration(
                "contracts/{contractId}/signed",
                HttpVerb.POST,
                NaiveAccessControl.Anonymous,
                async (
                    [FromServices] IDocumentService documentService,
                    [FromRoute] Guid contractId,
                    CancellationToken cancellationToken) =>
                {
                    await documentService.UploadSignedContract(
                        new SubmitContractFileDto(
                            contractId, 
                            new DocumentFileDto(new byte[0], $"{contractId}-signed.txt")), 
                        cancellationToken);
                    
                    return Results.StatusCode(StatusCodes.Status201Created);
                })
        };
    }
}