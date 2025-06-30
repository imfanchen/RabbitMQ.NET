using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

const string exchangeName = "pub-sub-logs";

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.ExchangeDeclareAsync(exchange: exchangeName, type: ExchangeType.Fanout);

QueueDeclareOk queueInstance = await channel.QueueDeclareAsync();
string queueName = queueInstance.QueueName;

await channel.QueueBindAsync(queue: queueName, exchange: exchangeName, routingKey: string.Empty);

Console.WriteLine(" [*] Waiting for logs.");

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += (model, eventArguments) =>
{
    byte[] body = eventArguments.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");
    return Task.CompletedTask;
};

await channel.BasicConsumeAsync(queue: queueName, autoAck: true, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();