namespace Example.Domain.Events;

public record SmsSuccessfullySent(string CustomerId) : IEvent
{
}