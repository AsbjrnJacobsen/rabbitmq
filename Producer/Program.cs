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

channel.ExchangeDeclare(exchange: "pubsub", type: ExchangeType.Fanout);


var message = $"Broadcasting this msg...";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish(exchange: "pubsub", "", null, body);

Console.WriteLine($"Send message {message}");