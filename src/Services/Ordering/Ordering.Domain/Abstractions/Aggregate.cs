
namespace Ordering.Domain.Abstractions;

public abstract class Aggregate<TID> : Entity<TID>, IAggregate<TID>
{
    private readonly List<IDomainEvent> _domainEvents=new(); 
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();


    public void AddDomainEvents(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public IDomainEvent[] ClearDomainEvents()
    {
        IDomainEvent[] dequeueDomainEvents=_domainEvents.ToArray();
        _domainEvents.Clear();
        return dequeueDomainEvents;
    }
}
