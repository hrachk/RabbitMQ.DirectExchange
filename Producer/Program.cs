// See https://aka.ms/new-console-template for more information
using RabbitMQ.Client;
using System;
using System.Text;

    Console.WriteLine("Hello, Producer!");

Task.Run(CreateTask(12000, "error"));
Task.Run(CreateTask(10000, "info"));
Task.Run(CreateTask(8000, "warning"));


Console.ReadKey();
        
static Func<Task> CreateTask(int timeToSleepTo, string routingKey)
{
    return () =>
    {
        var counter = 0;
        do
        {
            int timeToSleep = new Random().Next(1000, timeToSleepTo);
            Thread.Sleep(timeToSleep);

            var factory = new ConnectionFactory() { HostName = "localhost" };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: "direct_log", type: ExchangeType.Direct);

                var message = $"Message type [{routingKey}] from publisher N {counter++}";
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(exchange: "direct_log",
                        routingKey: routingKey,
                        basicProperties: null,
                        body: body);
                Console.WriteLine($"Message type [{routingKey}] is sent into Direct Exchange [N:{counter++}]");
            }
        }
        while (true);
    };
}