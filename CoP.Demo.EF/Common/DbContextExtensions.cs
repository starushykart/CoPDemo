using CoP.Demo.EF.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoP.Demo.EF.Common;

public static class DbContextExtensions
{
    public static async Task DispatchDomainEventsAsync(this DbContext dbContext, IPublisher? publisher, CancellationToken ct)
    {
        if (publisher == null)
            return;

        var domainEntries = dbContext.ChangeTracker
            .Entries<Entity>()
            .Where(x => x.Entity.DomainEvents.Count != 0)
            .ToList();

        var domainEvents = domainEntries.SelectMany(x => x.Entity.DomainEvents).ToList();
        domainEntries.ForEach(x => x.Entity.ClearEvents());

        foreach (var domainEvent in domainEvents)
            await publisher.Publish(domainEvent, ct);
    }
}