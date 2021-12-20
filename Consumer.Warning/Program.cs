// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

Console.WriteLine("Hello, Consumer.Warning!");

var factory = new ConnectionFactory() { HostName = "localhost" };
using (var connection = factory.CreateConnection())
using (var channel = connection.CreateModel())
{

    channel.ExchangeDeclare(exchange: "direct_log", type: ExchangeType.Direct);

    var queueName = channel.QueueDeclare().QueueName;

    channel.QueueBind(queue: queueName,
        exchange: "direct_log",
        routingKey: "warning");

    var consumer = new EventingBasicConsumer(channel);

    consumer.Received += (sender, args) =>
    {
        var body = args.Body;
        var message = Encoding.UTF8.GetString(body.ToArray());
        Console.WriteLine($"Receive message {message}");

    };

    channel.BasicConsume(queue: queueName,
        autoAck: true, consumer: consumer);

    Console.WriteLine($"Subscribed to the queue '{queueName}'");

    Console.ReadLine();
}