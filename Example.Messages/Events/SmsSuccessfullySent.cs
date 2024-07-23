namespace Example.Messages.Events;

public class SmsSuccessfullySent
{
    public SmsSuccessfullySent(string customerId, string telephoneNumber, string smsText)
    {
        if (string.IsNullOrWhiteSpace(customerId))
        {
            throw new ArgumentNullException(nameof(customerId));
        }

        if (string.IsNullOrWhiteSpace(telephoneNumber))
        {
            throw new ArgumentNullException(nameof(telephoneNumber));
        }

        if (string.IsNullOrWhiteSpace(smsText))
        {
            throw new ArgumentNullException(nameof(smsText));
        }

        CustomerId = customerId;
        TelephoneNumber = telephoneNumber;
        SmsText = smsText;
    }
    
    public string CustomerId { get; }
    
    public string TelephoneNumber { get; }
    
    public string SmsText { get; }
}