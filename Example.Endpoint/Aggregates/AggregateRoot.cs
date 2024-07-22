using SimpleEventStore;
using SimpleEventStore.InMemory;

namespace Example.Endpoint.Aggregates;

public class AggregateRoot()
{
    public AggregateRoot(IEnumerable<IEvent?> events) : this()
    {
        foreach (var @event in events)
        {
            switch (@event)
            {
                case SomethingHappened e:
                    Apply(e);
                    break;
                case SomethingElseHappened e:
                    Apply(e);
                    break;
                default:
                    throw new Exception();
            }

            Revision++;
        }
    }
    
    public void DoSomething()
    {
        var e = new SomethingHappened("123", "Hello");
        Apply(e);
        UncommittedEvents.Add(e);
    }

    public void DoSomethingElse()
    {
        var e = new SomethingElseHappened("Blah");
        Apply(e);
        UncommittedEvents.Add(e);
    }

    private void Apply(SomethingHappened e)
    {
        Id = e.Id;
        Message = e.Message;
    }

    private void Apply(SomethingElseHappened e)
    {
        Message = e.Message;
    }

    public string Id { get; private set; } = string.Empty;

    public string Message { get; private set; } = string.Empty;

    public int Revision { get; }

    public List<IEvent> UncommittedEvents { get; } = [];
}

public record SomethingHappened(string Id, string Message) : IEvent
{
}

public record SomethingElseHappened(string Message) : IEvent
{
}

public interface IEvent
{
}

public class DomainRepository
{
    private readonly EventStore _eventStore;

    public DomainRepository()
    {
        var storageEngine = new InMemoryStorageEngine();
        _eventStore = new EventStore(storageEngine);
    }

    public async Task<AggregateRoot> Load(string id)
    {
        var stream = await _eventStore.ReadStreamForwards(id);
        var events = stream.Select(storageEvent => storageEvent.EventBody as IEvent);
        return new AggregateRoot(events);
    }

    public async Task Save(AggregateRoot root)
    {
        var data = root.UncommittedEvents
            .Select(x => new EventData(Guid.NewGuid(), x))
            .ToArray();
        
        await _eventStore.AppendToStream(root.Id, root.Revision, data);
    }
}