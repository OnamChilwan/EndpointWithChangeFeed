using Example.Domain.Aggregates;
using Example.Domain.Events;
using SimpleEventStore;
using SimpleEventStore.InMemory;

namespace Example.Domain.Repositories;

public interface IDomainRepository
{
    Task<CustomerAggregate> Load(string id);

    Task Save(CustomerAggregate root);
}

public class DomainRepository : IDomainRepository
{
    private readonly EventStore _eventStore;

    public DomainRepository()
    {
        var storageEngine = new InMemoryStorageEngine();
        _eventStore = new EventStore(storageEngine);
    }

    public DomainRepository(IStorageEngine engine)
    {
        _eventStore = new EventStore(engine);
    }

    public async Task<CustomerAggregate> Load(string id)
    {
        var stream = await _eventStore.ReadStreamForwards(id);
        var events = stream.Select(storageEvent => storageEvent.EventBody as IEvent);
        return new CustomerAggregate(events);
    }

    public async Task Save(CustomerAggregate root)
    {
        var data = root.UncommittedEvents
            .Select(x => new EventData(Guid.NewGuid(), x))
            .ToArray();
        
        await _eventStore.AppendToStream(root.Id, root.Revision, data);
    }
}