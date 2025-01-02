using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

var factory = new ConnectionFactory
{
    HostName = "localhost",
    Port = 5672,
    UserName = "guest",
    Password = "guest"
};

using var connection = factory.CreateConnection();
using var channel = connection.CreateModel();



channel.ExchangeDeclare(exchange: "mainexchange2", type: ExchangeType.Direct);


var message = "this is the msg that might expire.";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("mainexchange2", "test", null, body);

Console.WriteLine($"Send msg: {message}");