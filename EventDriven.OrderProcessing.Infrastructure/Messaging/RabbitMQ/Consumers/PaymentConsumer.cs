using System.Text;
using System.Text.Json;
using EventDriven.OrderProcessing.Application.Payments.Commands;
using EventDriven.OrderProcessing.Infrastructure.Messaging.IntegrationEvents;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace EventDriven.OrderProcessing.Infrastructure.Messaging.RabbitMQ.Consumers;

public sealed class PaymentConsumer : BackgroundService
{
    private readonly RabbitMqConnectionProvider _provider;
    private readonly IServiceScopeFactory _scopeFactory;

    public PaymentConsumer(
        RabbitMqConnectionProvider provider,
        IServiceScopeFactory scopeFactory)
    {
        _provider = provider;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        var connection = await _provider.GetAsync();
        var channel = await connection.CreateChannelAsync();

        cancellationToken.Register(async () =>
        {
            await channel.CloseAsync();
            await connection.CloseAsync();
        });

        await channel.QueueDeclareAsync(
            queue: "order-created",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        var consumer = new AsyncEventingBasicConsumer(channel);

        consumer.ReceivedAsync += async (sender, args) =>
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            try
            {
                var body = Encoding.UTF8.GetString(args.Body.ToArray());
                var evt = JsonSerializer.Deserialize<OrderCreatedIntegrationEvent>(
                    body,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
                );

                if (evt is null)
                {
                    await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: false);
                    throw new InvalidOperationException("Invalid message");
                }

                await mediator.Send(
                    new ProcessPaymentCommand(evt.OrderId, evt.TotalAmount),
                    cancellationToken
                );

                await channel.BasicAckAsync(args.DeliveryTag, multiple: false);
            }
            catch
            {
                await channel.BasicNackAsync(args.DeliveryTag, multiple: false, requeue: true);
            }
        };

        await channel.BasicConsumeAsync(
            queue: "order-created",
            autoAck: false,
            consumer: consumer
        );
    }
}
