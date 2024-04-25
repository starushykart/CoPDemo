using CoP.Demo.EF.Domain.DomainEvents;
using CoP.Demo.EF.Messaging;
using MassTransit;
using MediatR;

namespace CoP.Demo.EF.EventHandlers;

public class SendSomewhereMessageHandler(IPublishEndpoint publishEndpoint) :
    INotificationHandler<OrderCreatedDomainEvent>
{
    public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        await publishEndpoint.Publish(new OrderCreated(notification.Order.Id), cancellationToken);
    }
}