using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using JetBrains.Annotations;


namespace TraceLens.Aspire;

[PublicAPI]
public static class TraceLensExtensions
{
    public static ContainerResource AddTraceLens(
        this IDistributedApplicationBuilder distributedApplicationBuilder, 
        int tracelensPort = 5001, 
        int plantUmlPort = 8080)
    {
        var tracelensDb = distributedApplicationBuilder
            .AddPostgres("tracelenspgsql")
            .AddDatabase("tracelensdb");

        var plantUml = distributedApplicationBuilder
            .AddContainer("plantuml", "plantuml/plantuml-server", tag: "tomcat")
            .WithHttpEndpoint(port: plantUmlPort, targetPort: 8080, name: "plantuml");

        distributedApplicationBuilder.Services.TryAddLifecycleHook<EnvironmentVariableHook>();
        
        var tracelens = distributedApplicationBuilder
            .AddContainer("tracelens", "rogeralsing/tracelens")
            .WithHttpEndpoint(port: tracelensPort, targetPort: 5001, name: "tracelens")
            .WithHttpEndpoint(port: 4317, targetPort: 4317, name: "otel")
            .WithEnvironment("PlantUml__RemoteUrl",plantUml.GetEndpoint("plantuml"))
            .WithReference(tracelensDb, "DefaultConnection");
        
        return tracelens.Resource;
    }
}