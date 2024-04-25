using MediatR;

namespace CoP.Demo.EF.Domain;

public interface IDomainEvent : INotification;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public Guid Id { get; set; }

    public IReadOnlyCollection<IDomainEvent> DomainEvents
        => _domainEvents.AsReadOnly();

    public void RaiseEvent(IDomainEvent domainEvent)
        => _domainEvents.Add(domainEvent);

    public void ClearEvents()
        => _domainEvents.Clear();
}