using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

ConnectionFactory factory = new();
using IConnection connection = await factory.CreateConnectionAsync();
using IChannel channel = await connection.CreateChannelAsync();

await channel.QueueDeclareAsync(queue: "task-queue", durable: true, exclusive: false, autoDelete: false, arguments: null);

await channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

Console.WriteLine(" [*] Waiting for messages.");

AsyncEventingBasicConsumer consumer = new(channel);
consumer.ReceivedAsync += async (ModuleHandle, eventArguments) =>
{
    byte[] body = eventArguments.Body.ToArray();
    string message = Encoding.UTF8.GetString(body);
    Console.WriteLine($" [x] Received {message}");

    int dots = message.Count(c => c == '.');
    Console.WriteLine($" [x] Processing...");
    await Task.Delay(dots * 1000);
    Console.WriteLine($" [x] Done!");

    await channel.BasicAckAsync(deliveryTag: eventArguments.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queue: "task-queue", autoAck: false, consumer: consumer);

Console.WriteLine(" Press [enter] to exit.");
Console.ReadLine();