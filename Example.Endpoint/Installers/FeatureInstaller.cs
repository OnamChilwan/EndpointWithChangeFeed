using Example.Endpoint.Configuration;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Endpoint.Installers;

public class FeatureInstaller
{
    public void Install(IServiceCollection services, CosmosDbSettings cosmosDbSettings)
    {
        InstallExternalDependencies(services, cosmosDbSettings);
    }

    protected virtual void InstallExternalDependencies(IServiceCollection services, CosmosDbSettings cosmosDbSettings)
    {
        services.AddSingleton(new CosmosClient(cosmosDbSettings.CosmosDbEndpoint, cosmosDbSettings.CosmosDbKey));
    }
}