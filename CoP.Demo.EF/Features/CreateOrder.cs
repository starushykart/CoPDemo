using CoP.Demo.EF.Domain;
using CoP.Demo.EF.Domain.DomainEvents;
using CoP.Demo.EF.Messaging;
using MassTransit;
using MassTransit.Courier.Contracts;
using MediatR;

namespace CoP.Demo.EF.Features;

public record CreateOrderCommand : IRequest<Guid>;

public class CreateOrderCommandHandler(ApplicationDbContext context, IPublishEndpoint publishEndpoint) 
    : IRequestHandler<CreateOrderCommand, Guid>
{
    public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = Order.Create();
        order.RaiseEvent(new OrderCreatedDomainEvent(order));

        context.Orders.Add(order);
        
        await context.SaveChangesAsync(cancellationToken);
        
        return order.Id;
    }
}