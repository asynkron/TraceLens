using Aspire.Hosting.Lifecycle;
using Microsoft.Extensions.Logging;

namespace eShopLite.AppHost;

public class EnvironmentVariableHook : IDistributedApplicationLifecycleHook
{
    private readonly ILogger<EnvironmentVariableHook> _logger;

    public EnvironmentVariableHook(ILogger<EnvironmentVariableHook> logger)
    {
        _logger = logger;
    }
    public Task AfterEndpointsAllocatedAsync(DistributedApplicationModel appModel, CancellationToken cancellationToken)
    {
        var resources = appModel.GetProjectResources();
        
        var endpoint = GetOtelEndpoint(appModel);
        if (endpoint == null)
        {
            _logger.LogWarning("No endpoint for the collector");
            return Task.CompletedTask;
        }

        if (resources.Count() == 0)
        {
            _logger.LogInformation("No resources to add Environment Variables to");
        }

        foreach (var resourceItem in resources)
        {
            _logger.LogDebug($"Forwarding Telemetry for {resourceItem.Name} to the collector");
            if (resourceItem == null) continue;

            resourceItem.Annotations.Add(new EnvironmentCallbackAnnotation(context =>
            {
                if (context.EnvironmentVariables.ContainsKey("OTEL_EXPORTER_OTLP_ENDPOINT"))
                    context.EnvironmentVariables.Remove("OTEL_EXPORTER_OTLP_ENDPOINT");
                context.EnvironmentVariables.Add("OTEL_EXPORTER_OTLP_ENDPOINT", endpoint.UriString);
            }));
        }

        return Task.CompletedTask;
    }

    private static EndpointReference GetOtelEndpoint(DistributedApplicationModel appModel)
    {
        var traceLensResource =
            appModel.Resources.OfType<IResourceWithEndpoints>().First(c => c.Name == "tracelens");

        return traceLensResource.GetEndpoint("otel");    }
}