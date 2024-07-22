using Example.Endpoint.Aggregates;
using Example.Endpoint.Events;
using SimpleEventStore;
using SimpleEventStore.InMemory;

namespace Example.Endpoint.Repositories;

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