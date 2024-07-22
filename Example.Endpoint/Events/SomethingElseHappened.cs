namespace Example.Endpoint.Events;

public record SomethingElseHappened(string Message) : IEvent
{
}