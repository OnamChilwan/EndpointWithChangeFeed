using Example.Domain.Aggregates;
using Example.Domain.Repositories;
using Example.Endpoint.Configuration;
using Example.Endpoint.Installers;
using Example.Messages.Commands;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wolverine;
using Wolverine.AzureServiceBus;

// TODO: This is test code
// var repo = new DomainRepository();
//
// var ag = new CustomerAggregate();
// ag.DoSomething();
// await repo.Save(ag);
//
// var x = await repo.Load("123");
// x.DoSomethingElse();
// await repo.Save(x);
//
// var y = await repo.Load("123");

using var host = Host
    .CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        services.AddLogging();
        
        var cosmosDbSettings = context.Configuration.GetSection(nameof(CosmosDbSettings)).Get<CosmosDbSettings>();
        new FeatureInstaller().Install(services, cosmosDbSettings!);
    })
    .UseWolverine((context, options) =>
    {
        var applicationSettings = context.Configuration.GetSection(nameof(ApplicationSettings)).Get<ApplicationSettings>();
        options.ConfigureEndpoint(applicationSettings!);
        
        options.PublishMessage<SendSmsCommand>().ToAzureServiceBusQueue("foo-queue"); // TODO: delete
    })
    .Build();

// await host.StartAsync();
// var bus = host.Services.GetRequiredService<IMessageBus>();
// await bus.SendAsync(new HelloWorldCommand { Message = "hello world" });

await host.RunAsync();