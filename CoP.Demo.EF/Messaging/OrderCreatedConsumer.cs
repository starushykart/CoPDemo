using MassTransit;

namespace CoP.Demo.EF.Messaging;

public record OrderCreated(Guid Id);

public class OrderCreatedConsumer(ILogger<OrderCreatedConsumer> logger) : IConsumer<OrderCreated>
{
    public Task Consume(ConsumeContext<OrderCreated> context)
    {
        logger.LogInformation("Order created message consumed");
        return Task.CompletedTask;
    }
}