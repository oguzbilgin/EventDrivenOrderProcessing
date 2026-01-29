using EventDriven.OrderProcessing.Domain.Common;
using EventDriven.OrderProcessing.Domain.Orders;
using EventDriven.OrderProcessing.Domain.Users;
using EventDriven.OrderProcessing.Infrastructure.Messaging;
using Microsoft.EntityFrameworkCore;

namespace EventDriven.OrderProcessing.Infrastructure.Persistence;

public sealed class AppDbContext : DbContext
{
    private readonly DomainEventDispatcher _domainEventDispatcher;
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<User> Users => Set<User>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();

    public AppDbContext(DbContextOptions options , DomainEventDispatcher domainEvenDispatcher) : base(options)
    {
        _domainEventDispatcher = domainEvenDispatcher;
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var domainEvents = ChangeTracker
            .Entries<BaseEntity>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);

        if (domainEvents.Count > 0)
        {
            await _domainEventDispatcher
                .DispatchAsync(domainEvents, cancellationToken);
        }

        foreach (var entry in ChangeTracker.Entries<BaseEntity>())
        {
            entry.Entity.ClearDomainEvents();
        }

        return result;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }
}
