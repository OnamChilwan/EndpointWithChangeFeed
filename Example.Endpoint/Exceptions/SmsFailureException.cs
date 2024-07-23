namespace Example.Endpoint.Exceptions;

public class SmsFailureException(string message, Exception exception)
    : Exception($"Failed to obtain a successful response when sending SMS with reason {message}", exception);