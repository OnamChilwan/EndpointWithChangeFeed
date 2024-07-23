namespace Example.Messages.Commands;

public class SendSmsCommand
{
    public required string CustomerId { get; set; }
    
    public required string TelephoneNumber { get; set; }
    
    public required string SmsText { get; set; }
}