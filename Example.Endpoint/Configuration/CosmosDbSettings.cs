namespace Example.Endpoint.Configuration;

public class CosmosDbSettings
{
    public required string CosmosDbEndpoint { get; set; }
    
    public required string CosmosDbKey { get; set; }

    public const string DatabaseName = "pricedb";

    public const string ContainerName = "data";
}