using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: dotnet run [debug] [info] [warning] [error]");
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

const string exchangeName = "routing-logs";

ConnectionFactory factory = new();
IConnection connection = await factory.CreateConnectionAsync();
IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);

QueueDeclareOk queueInstance = await channel.QueueDeclareAsync();
string queueName = queueInstance.QueueName;

foreach (string severity in args)
{
    await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: severity);
}

Console.WriteLine(" [*] Waiting for logs.");

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += (model, exchangeArguments) =>
{
    string routingKey = exchangeArguments.RoutingKey;
    byte[] body = exchangeArguments.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {routingKey}: {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();