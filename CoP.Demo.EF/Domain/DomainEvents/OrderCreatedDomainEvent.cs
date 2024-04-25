namespace CoP.Demo.EF.Domain.DomainEvents;

public record OrderCreatedDomainEvent(Order Order) : IDomainEvent;