using Example.Endpoint.Commands;
using Example.Endpoint.Configuration;
using Example.Endpoint.Entities;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Wolverine.Attributes;

namespace Example.Endpoint.Handlers;

[WolverineHandler]
public class HelloWorldHandler(ILogger<HelloWorldHandler> logger, CosmosClient cosmosClient) : IHandle<HelloWorldCommand>
{
    public async Task HandleAsync(HelloWorldCommand message)
    {
        logger.LogInformation("Greeting message {message}", message.Message);
        
        var container = cosmosClient.GetContainer(CosmosDbSettings.DatabaseName, CosmosDbSettings.ContainerName);
        await container.UpsertItemAsync(new Entity
        {
            Id = Guid.NewGuid().ToString(),
            Message = message.Message
        });
    }
}