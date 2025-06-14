using System.Text;
using System.Text.Json;
using RabbitMQ.Client;

const string queueName = "basic-queue";

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

var messageObject = new
{
    Sender = "Fan Chen",
    Model = "o4-mini",
    Text = "What is the benefit of using RabbitMQ?"
};
JsonSerializerOptions serializeOptions = new()
{
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    WriteIndented = true
};
string message = JsonSerializer.Serialize(messageObject, serializeOptions);
byte[] body = Encoding.UTF8.GetBytes(message);

await channel.BasicPublishAsync(exchange: string.Empty, routingKey: queueName, body: body);
Console.WriteLine($"[X] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();