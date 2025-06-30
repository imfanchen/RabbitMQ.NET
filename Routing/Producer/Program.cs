using System.Text;
using RabbitMQ.Client;

string[] allowedLevels = { "debug", "info", "warning", "error" };
if (args.Length == 0 || !allowedLevels.Contains(args[0], StringComparer.OrdinalIgnoreCase))
{
    Console.WriteLine("Invalid or missing log level. Please use one of: debug, info, warning, error.");
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
    Environment.ExitCode = 1;
    return;
}

const string exchangeName = "routing-logs";

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Direct);

string severity = args.Length > 0 ? args[0] : "debug";
string message = args.Length > 1 ? string.Join(" ", args.Skip(1).ToArray()) : "Hello World!";
byte[] body = Encoding.UTF8.GetBytes(message);
await channel.BasicPublishAsync(exchange: exchangeName, routingKey: severity, body: body);
Console.WriteLine($" [x] Sent '{severity}':'{message}'");


Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();