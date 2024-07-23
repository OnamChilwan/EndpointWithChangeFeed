using System.Net.Http.Json;
using Example.Domain.Models;
using Example.Domain.Repositories;
using Example.Endpoint.Configuration;
using Example.Endpoint.Exceptions;
using Example.Messages.Commands;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Wolverine.Attributes;

namespace Example.Endpoint.Handlers;

[WolverineHandler]
public class SendSmsCommandHandler(
    ILogger<SendSmsCommandHandler> logger,
    ISmsServiceClient smsServiceClient,
    IDomainRepository domainRepository) : IHandle<SendSmsCommand>
{
    public async Task HandleAsync(SendSmsCommand message)
    {
        logger.LogInformation("Handling message for customer {customerId}", message.CustomerId);

        // TODO: OPTION 1 
        // Handle message
        // Load aggregate
        // Check if customer has been notified
        // If not, send SMS message
        // Acknowledge this in aggregate
        // Save aggregate
        // Publish event

        // TODO: OPTION 2
        // Handle message
        // Save entity to cosmos
        // Change feed processor
        // Load aggregate
        // Check if customer has been notified
        // If not, send SMS message
        // Acknowledge this in aggregate
        // Save aggregate

        var customerAggregate = await domainRepository.Load(message.CustomerId);

        if (customerAggregate.HasReceivedNotification)
        {
            logger.LogInformation("SMS already sent to customer {customerId}", message.CustomerId);
            return;
        }
        
        logger.LogInformation("Sending SMS to customer {customerId}", message.CustomerId);
        var smsResult = await smsServiceClient.SendSms(null);
        customerAggregate.CustomerNotified(new CustomerNotificationResult(smsResult.IsSuccess, message.CustomerId, smsResult.Message));
        await domainRepository.Save(customerAggregate);

        // var entity = new SmsEntity
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Message = message.SmsText,
        //     TelephoneNumber = message.TelephoneNumber
        // };
        // await createSmsCommand.ExecuteAsync(entity);

        // logger.LogInformation("Document saved for customer {customerId} and documentId {id}");

        // var container = cosmosClient.GetContainer(CosmosDbSettings.DatabaseName, CosmosDbSettings.ContainerName);
        // await container.UpsertItemAsync(new Entity
        // {
        //     Id = Guid.NewGuid().ToString(),
        //     Message = message.Message
        // });
    }
}

public class SmsServiceResponse
{
    public required bool IsSuccess { get; set; }
    
    public required string Message { get; set; }
}

public interface ISmsServiceClient
{
    Task<SmsServiceResponse> SendSms(object message);
}

public class SmsServiceClient(HttpClient httpClient, ILogger<SmsServiceClient> logger)
    : ISmsServiceClient // TODO: this client needs to be faked out
{
    public async Task<SmsServiceResponse> SendSms(object message)
    {
        try
        {
            var httpResponse = await httpClient.PostAsJsonAsync("", message);

            httpResponse.EnsureSuccessStatusCode();
            
            // TODO: is there a mock model I can use?
            
            return new SmsServiceResponse
            {
                Message = $"Message successfully sent with reference", // TODO: complete this
                IsSuccess = true
            };
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            throw new SmsFailureException(e.Message, e);
        }
    }
}


// public interface ICreateSmsCommand
// {
//     Task ExecuteAsync(SmsEntity entity);
// }

// public class CosmosCreateSmsCommand(CosmosClient cosmosClient) : ICreateSmsCommand
// {
//     public async Task ExecuteAsync(SmsEntity entity)
//     {
//         var container = cosmosClient.GetContainer(CosmosDbSettings.DatabaseName, CosmosDbSettings.ContainerName);
//         await container.UpsertItemAsync(entity);
//     }
// }

// public class SmsEntity
// {
//     [JsonProperty("id")] public required string Id { get; set; }
//
//     [JsonProperty("telephoneNumber")] public required string TelephoneNumber { get; set; }
//
//     [JsonProperty("message")] public required string Message { get; set; }
// }