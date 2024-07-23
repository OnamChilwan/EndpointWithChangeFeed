using Example.Domain.Events;
using Example.Domain.Models;

namespace Example.Domain.Aggregates;

public class CustomerAggregate()
{
    public CustomerAggregate(IEnumerable<IEvent?> events) : this()
    {
        foreach (var @event in events)
        {
            switch (@event)
            {
                case SmsSuccessfullySent e:
                    Apply(e);
                    break;
                case SmsFailed e:
                    Apply(e);
                    break;
                default:
                    throw new Exception();
            }

            Revision++;
        }
    }

    public void CustomerNotified(CustomerNotificationResult customerNotificationResult)
    {
        if (customerNotificationResult.Success)
        {
            var e = new SmsSuccessfullySent(customerNotificationResult.CustomerId);
            Apply(e);
            UncommittedEvents.Add(e);
            return;
        }

        var f = new SmsFailed(customerNotificationResult.CustomerId, customerNotificationResult.Message);   
        Apply(f);
        UncommittedEvents.Add(f);
    }

    private void Apply(SmsSuccessfullySent e)
    {
        Id = e.CustomerId;
        HasReceivedNotification = true;
    }
    
    private void Apply(SmsFailed e)
    {
        Id = e.CustomerId;
        HasReceivedNotification = false;
    }

    public string Id { get; private set; } = string.Empty;
    
    public bool HasReceivedNotification { get; private set; }
    
    public int Revision { get; }

    public List<IEvent> UncommittedEvents { get; } = [];
}