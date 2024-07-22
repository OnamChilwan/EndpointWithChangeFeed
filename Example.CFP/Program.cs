// See https://aka.ms/new-console-template for more information

using Microsoft.Azure.Cosmos;

const string databaseName = "";
const string sourceContainerName = "";
Container leaseContainer = null;

var cosmosClient = new CosmosClient("", "");
var changeFeedProcessor = cosmosClient.GetContainer(databaseName, sourceContainerName)
    .GetChangeFeedProcessorBuilder<Entity>("<name-for-the-workflow>", HandleChangesAsync)
    .WithInstanceName("<name-for-the-host-instance>")
    .WithLeaseContainer(leaseContainer)
    .Build();

await changeFeedProcessor.StartAsync();

async Task HandleChangesAsync(
    ChangeFeedProcessorContext context,
    IReadOnlyCollection<Entity> changes,
    CancellationToken cancellationToken)
{
    
}

Console.WriteLine("Hello, World!");

public class Entity
{
}