using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 5672,
    UserName = "admin",
    Password = "admin"
};

using var connection = factory.CreateConnection();

using var channel = connection.CreateModel();

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);

var queName = channel.QueueDeclare().QueueName;

var consumer = new EventingBasicConsumer(channel);

channel.QueueBind(queue: queName, exchange: "pubsub", routingKey: "");

consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"SecondConsumer - Received: {message}");
};

channel.BasicConsume(queue: queName, autoAck: true, consumer: consumer);

Console.WriteLine("Consuming...");
Console.ReadKey();