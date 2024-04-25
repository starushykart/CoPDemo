using CoP.Demo.EF.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoP.Demo.EF.Features;

public record UpdateOrderCommand(Guid Id, Status Status) : IRequest;

public class UpdateOrderCommandHandler(ApplicationDbContext context) : IRequestHandler<UpdateOrderCommand>
{
    public async Task Handle(UpdateOrderCommand command, CancellationToken cancellationToken)
    {
        var order = await context.Orders
            .FirstAsync(x => x.Id == command.Id, cancellationToken);
        
        order.Update(command.Status);
        context.AuditLogs.Add(AuditLog.Create("Order updated"));

        await context.SaveChangesAsync(cancellationToken);
    }
}
