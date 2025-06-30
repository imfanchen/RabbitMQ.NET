using System.Text;
using RabbitMQ.Client;

const string exchangeName = "crypto-prices";

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Topic);

string routingKey = (args.Length > 0) ? args[0] : "coinbase.btc-usd";

// In real world scenarios, the prices should be fetched from exchange's api
string message = (args.Length > 1) ? args[1] : "100,000";
byte[] body = Encoding.UTF8.GetBytes(message);
await channel.BasicPublishAsync(exchange: exchangeName, routingKey: routingKey, body: body);
Console.WriteLine($" [x] Sent '{routingKey}':'{message}'");

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();