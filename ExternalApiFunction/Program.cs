using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace Repro.Api.Function;

[ExcludeFromCodeCoverage(Justification = "Unnecessary to test the Program.cs")]
public static class Program
{
    public static async Task Main()
    {
        var host = new HostBuilder()
            .ConfigureFunctionsWorkerDefaults(builder =>
            {
                builder
                    .AddApplicationInsights()
                    .AddApplicationInsightsLogger();
            })
            .ConfigureAppConfiguration((hostContext, config) =>
            {
                config.AddJsonFile("host.json");
                config.AddEnvironmentVariables();
                config.Build();
            })
            .ConfigureServices(
                (hostContext, services) =>
                {
                    services.AddTransient((s) => { return hostContext.Configuration; });
                })
            .Build();

        await host.RunAsync();
    }
}
