namespace Example.Domain.Events;

public record SmsFailed(string CustomerId, string FailureMessage) : IEvent
{
}