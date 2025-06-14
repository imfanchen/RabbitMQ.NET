using System.Text;
using RabbitMQ.Client;

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "task-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

string message = GetMessage(args);
byte[] body = Encoding.UTF8.GetBytes(message);

BasicProperties properties = new() { Persistent = true };

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: "task-queue", mandatory: true, basicProperties: properties, body: body);
Console.WriteLine($" [x] Sent {message}");

static string GetMessage(string[] args)
{
    return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
}