namespace Example.Endpoint.Events;

public record SomethingHappened(string Id, string Message) : IEvent
{
}