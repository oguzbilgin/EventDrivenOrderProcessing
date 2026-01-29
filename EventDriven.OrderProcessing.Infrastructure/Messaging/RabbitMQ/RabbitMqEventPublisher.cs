using System.Text;
using System.Text.Json;
using EventDriven.OrderProcessing.Application.Common.Interfaces;
using RabbitMQ.Client;

namespace EventDriven.OrderProcessing.Infrastructure.Messaging.RabbitMQ;

public sealed class RabbitMqEventPublisher : IIntegrationEventPublisher
{
    private readonly RabbitMqConnectionProvider _provider;

    public RabbitMqEventPublisher(RabbitMqConnectionProvider provider)
    {
        _provider = provider;
    }

    public async Task PublishAsync<T>(T @event, string queueName)
    {
        var connection = await _provider.GetAsync();
        await using var channel = await connection.CreateChannelAsync();

        await channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(@event));

        var props = new BasicProperties
        {
            DeliveryMode = DeliveryModes.Persistent,
            ContentType = "application/json"
        };

        await channel.BasicPublishAsync(
            exchange: "",
            routingKey: queueName,
            mandatory: false,
            basicProperties: props,
            body: body);
    }
}
