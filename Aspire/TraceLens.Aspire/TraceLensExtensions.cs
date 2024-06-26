﻿using Aspire.Hosting;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.Lifecycle;
using JetBrains.Annotations;


namespace TraceLens.Aspire;

[PublicAPI]
public static class TraceLensExtensions
{
    public static ContainerResource AddTraceLens(this IDistributedApplicationBuilder distributedApplicationBuilder)
    {
        var tracelensDb = distributedApplicationBuilder.AddPostgres("tracelenspgsql").AddDatabase("tracelensdb");

        var plantUml = distributedApplicationBuilder.AddContainer("plantuml", "plantuml/plantuml-server", tag: "tomcat")
            .WithHttpEndpoint(8080, name: "plantuml");

        distributedApplicationBuilder.Services.TryAddLifecycleHook<EnvironmentVariableHook>();
        
        var tracelens = distributedApplicationBuilder.AddContainer("tracelens", "rogeralsing/tracelens")
            .WithHttpEndpoint(5001, name: "tracelens")
            .WithHttpEndpoint(4317, name: "otel")
            .WithEnvironment("PlantUml__RemoteUrl",plantUml.GetEndpoint("plantuml"))
            .WithReference(tracelensDb, "DefaultConnection");
        
        return tracelens.Resource;
    }
}