using CoP.Demo.EF.Common;
using CoP.Demo.EF.Domain;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CoP.Demo.EF;

public sealed class ApplicationDbContext(IPublisher publisher, DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();
    
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await this.DispatchDomainEventsAsync(publisher, cancellationToken);
        return await base.SaveChangesAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}