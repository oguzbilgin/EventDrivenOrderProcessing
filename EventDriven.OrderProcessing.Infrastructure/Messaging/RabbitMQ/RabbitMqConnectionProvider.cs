
using RabbitMQ.Client;

namespace EventDriven.OrderProcessing.Infrastructure.Messaging.RabbitMQ;

public sealed class RabbitMqConnectionProvider : IAsyncDisposable
{
    private readonly ConnectionFactory _factory;
    private IConnection? _connection;

    public RabbitMqConnectionProvider()
    {
        _factory = new ConnectionFactory
        {
            HostName = "localhost"
        };
    }

    public async Task<IConnection> GetAsync()
    {
        if (_connection is not null && _connection.IsOpen)
            return _connection;

        _connection = await _factory.CreateConnectionAsync();
        return _connection;
    }

    public async ValueTask DisposeAsync()
    {
        if (_connection is not null)
            await _connection.DisposeAsync();
    }
}
