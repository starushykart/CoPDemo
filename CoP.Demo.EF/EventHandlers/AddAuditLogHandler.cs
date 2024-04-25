using CoP.Demo.EF.Domain;
using CoP.Demo.EF.Domain.DomainEvents;
using MediatR;

namespace CoP.Demo.EF.EventHandlers;

public class AddAuditLogHandler(ApplicationDbContext context) :
    INotificationHandler<OrderCreatedDomainEvent>,
    INotificationHandler<OrderUpdatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        context.AuditLogs.Add(AuditLog.Create("Order created"));
    }

    public async Task Handle(OrderUpdatedDomainEvent notification, CancellationToken cancellationToken)
    {
        context.AuditLogs.Add(AuditLog.Create("Order updated"));
    }
}