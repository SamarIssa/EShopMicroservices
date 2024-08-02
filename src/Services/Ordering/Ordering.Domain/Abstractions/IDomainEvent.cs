namespace Ordering.Domain.Abstractions;

//We use notification to allow domain events to be dispatched through mediator handlers
public interface IDomainEvent:INotification
{
    Guid EventId=> Guid.NewGuid();
    public DateTime OccurredOn=> DateTime.UtcNow;
    public string EventType => GetType().AssemblyQualifiedName;
}
