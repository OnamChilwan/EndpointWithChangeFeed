namespace Example.Endpoint.Configuration;

public class ApplicationSettings
{
    public required string QueueName { get; set; }
    
    public required string ServiceBusConnectionString { get; set; }
}