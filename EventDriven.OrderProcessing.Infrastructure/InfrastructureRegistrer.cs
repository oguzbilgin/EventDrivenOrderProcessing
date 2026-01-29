using EventDriven.OrderProcessing.Application.Auth.Interfaces;
using EventDriven.OrderProcessing.Application.Common.Interfaces;
using EventDriven.OrderProcessing.Application.Orders.EventHandlers;
using EventDriven.OrderProcessing.Application.Orders.Queries.Services;
using EventDriven.OrderProcessing.Domain.Orders.Events;
using EventDriven.OrderProcessing.Infrastructure.Auth;
using EventDriven.OrderProcessing.Infrastructure.Identity;
using EventDriven.OrderProcessing.Infrastructure.Messaging;
using EventDriven.OrderProcessing.Infrastructure.Messaging.RabbitMQ;
using EventDriven.OrderProcessing.Infrastructure.Messaging.RabbitMQ.Consumers;
using EventDriven.OrderProcessing.Infrastructure.Persistence;
using EventDriven.OrderProcessing.Infrastructure.Persistence.Queries;
using EventDriven.OrderProcessing.Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EventDriven.OrderProcessing.Infrastructure;

public static class InfrastructureRegistrer
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Default");

        services.AddSingleton<RabbitMqConnectionProvider>();
        services.AddSingleton<IIntegrationEventPublisher, RabbitMqEventPublisher>();
        services.AddHostedService<PaymentConsumer>();

        services.AddDbContext<AppDbContext>(options => 
            options.UseSqlServer(connectionString));

        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<DomainEventDispatcher>();
        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderQueryService, OrderQueryService>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        services.AddScoped<ICurrentUser, CurrentUser>();
        services.AddScoped<
            IDomainEventHandler<OrderCreatedDomainEvent>,
            OrderCreatedDomainEventHandler>();


        return services;
    }
}
