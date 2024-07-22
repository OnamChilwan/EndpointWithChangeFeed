using Example.Endpoint.Aggregates;
using Example.Endpoint.Events;
using FluentAssertions;

namespace Example.Endpoint.UnitTests;

public class AggregateRootTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void Given_No_Stream_Exists_When_Doing_Something_Then_State_Is_Updated()
    {
        var subject = new AggregateRoot();
        subject.DoSomething();

        subject.UncommittedEvents.Should().Contain(x => x is SomethingHappened);
    }

    [Test]
    public void Given_Stream_Exists_When_Aggregate_Is_Hydrated_Then_Its_State_Is_Correct()
    {
        var events = new IEvent[]
        {
            new SomethingHappened("123", "Testing"),
            new SomethingElseHappened("Test")
        };
        var subject = new AggregateRoot(events);

        subject.Id.Should().Be("123");
        subject.Message.Should().Be("Test");
        subject.UncommittedEvents.Should().BeEmpty();
    }
}