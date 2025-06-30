using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string exchangeName = "crypto-prices";

if (args.Length < 1)
{
    Console.Error.WriteLine("Usage: dotnet run [<exchange>.<symbol>]...");
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);

QueueDeclareOk queueInstance = await channel.QueueDeclareAsync();
string queueName = queueInstance.QueueName;

foreach (string bindingKey in args)
{
    await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: bindingKey);
}

Console.WriteLine(" [*] Waiting for messages.");

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += (model, eventArguments) =>
{
    string routingKey = eventArguments.RoutingKey;
    byte[] body = eventArguments.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received '{routingKey}': '{message}'");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: queueName, consumer: consumer, autoAck: true);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();