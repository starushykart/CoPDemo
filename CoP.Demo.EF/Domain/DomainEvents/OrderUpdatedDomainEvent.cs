namespace CoP.Demo.EF.Domain.DomainEvents;

public record OrderUpdatedDomainEvent(Order Order) : IDomainEvent;