namespace Example.Domain.Models;

public record CustomerNotificationResult(bool Success, string CustomerId, string Message)
{
}