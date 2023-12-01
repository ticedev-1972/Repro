using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Abstractions;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.OpenApi.Models;
using System.Diagnostics.CodeAnalysis;

namespace Repro.Api.Function.OpenApiSpec;

[ExcludeFromCodeCoverage()]
public class OpenApiConfigurationOptions : IOpenApiConfigurationOptions
{
    public OpenApiInfo Info { get; set; } = new OpenApiInfo { };
    public List<OpenApiServer> Servers { get; set; } = new();
    public OpenApiVersionType OpenApiVersion { get; set; } = OpenApiVersionType.V3;
    public bool IncludeRequestingHostName { get; set; } = false;
    public bool ForceHttp { get; set; } = true;
    public bool ForceHttps { get; set; } = false;
    public List<IDocumentFilter>? DocumentFilters { get; set; }
}
