using Example.Domain.Aggregates;
using Example.Domain.Models;
using Example.Domain.Repositories;
using FluentAssertions;
using SimpleEventStore.InMemory;

namespace Example.Endpoint.UnitTests;

public class DomainRepositoryTests
{
    private DomainRepository _subject = null!;

    [SetUp]
    public void Setup()
    {
        _subject = new DomainRepository(new InMemoryStorageEngine());
    }
    
    [Test]
    public async Task Given_Aggregate_Exists_When_Loading_Then_Aggregate_Is_Loaded_Correctly()
    {
        const string id = "123";
        var aggregateRoot = new CustomerAggregate();
        
        aggregateRoot.CustomerNotified(new CustomerNotificationResult(true, "123", "Message"));

        await _subject.Save(aggregateRoot);

        var result = await _subject.Load(id);
        
        result.Id.Should().Be("123");
        result.HasReceivedNotification.Should().BeTrue();
        result.UncommittedEvents.Should().BeEmpty();
    }

    [Test]
    public async Task Given_Aggregate_Does_Not_Exist_When_Loading_Then_Empty_Aggregate_Is_Returned()
    {
        var result = await _subject.Load("123");

        result.UncommittedEvents.Should().BeEmpty();
        result.Id.Should().BeEmpty();
    }
}