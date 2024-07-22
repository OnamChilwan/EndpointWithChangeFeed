using Example.Endpoint.Configuration;
using Wolverine;
using Wolverine.AzureServiceBus;
using Wolverine.FluentValidation;

namespace Example.Endpoint.Installers;

public static class EndpointInstaller
{
    public static void ConfigureEndpoint(this WolverineOptions options, ApplicationSettings settings)
    {
        options.ApplicationAssembly = typeof(Program).Assembly;
        options.UseFluentValidation();
        
        options
            .UseAzureServiceBus(settings.ServiceBusConnectionString)
            .SystemQueuesAreEnabled(false)
            .ConfigureListeners(config =>
            {
                config.ConfigureDeadLetterQueue("error");
            });

        options.ListenToAzureServiceBusQueue(settings.QueueName);
    }
}