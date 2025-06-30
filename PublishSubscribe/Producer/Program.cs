using System.Text;
using RabbitMQ.Client;

const string exchangeName = "pub-sub-logs";

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);

Func<string[], string> getMessage = args => args.Length > 0 ? string.Join(" ", args) : "Hello, World!";
string message = getMessage(args);

byte[] body = Encoding.UTF8.GetBytes(message);
await channel.BasicPublishAsync(exchange: exchangeName, routingKey: string.Empty, body: body);

Console.WriteLine($" [x] Sent {message}");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();