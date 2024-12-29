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

channel.ExchangeDeclare(exchange: "myroutingexchange", type: ExchangeType.Direct);

var message = "This message needs to be routed.";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("myroutingexchange", "paymentsonly", null, body);
Console.WriteLine($"Send msg: {message}");