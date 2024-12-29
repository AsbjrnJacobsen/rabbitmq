using System;
using System.Text;
using RabbitMQ.Client;

var factory = new ConnectionFactory
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "admin",
                Password = "admin"
            };

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.QueueDeclare(queue: "letterbox", durable: false, exclusive: false, autoDelete: false, arguments: null);

var random = new Random();

var messageId = 1;

while (true)
{
    var publishingTime = random.Next(1, 4);
    
    var message = $"Sending messageId: {messageId++}";

    var encodedMessage = Encoding.UTF8.GetBytes(message);

    channel.BasicPublish("", "letterbox", null, encodedMessage);

    Console.WriteLine($"Published message {message}");
    Task.Delay(TimeSpan.FromSeconds(publishingTime)).Wait();
}

