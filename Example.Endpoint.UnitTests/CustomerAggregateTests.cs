using Example.Domain.Aggregates;
using Example.Domain.Events;
using Example.Domain.Models;
using FluentAssertions;

namespace Example.Endpoint.UnitTests;

public class CustomerAggregateTests
{
    private const string CustomerId = "123";
    private CustomerAggregate _subject = null!;
    
    [SetUp]
    public void Setup()
    {
        _subject = new CustomerAggregate();
    }

    [Test]
    public void Given_No_Stream_Exists_When_Customer_Has_Been_Notified_Successfully_Then_State_Is_Updated()
    {
        _subject.CustomerNotified(new CustomerNotificationResult(true, CustomerId, "Success"));
        _subject.UncommittedEvents.Should().Contain(x => x is SmsSuccessfullySent);
    }
    
    [Test]
    public void Given_No_Stream_Exists_When_Customer_Has_Failed_To_Be_Notified_Then_State_Is_Updated()
    {
        _subject.CustomerNotified(new CustomerNotificationResult(false, CustomerId, "Failed"));
        _subject.UncommittedEvents.Should().Contain(x => x is SmsFailed);
    }
}