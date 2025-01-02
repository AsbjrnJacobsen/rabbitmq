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



channel.ExchangeDeclare(exchange: "mainexchange", type: ExchangeType.Direct, arguments: new Dictionary<string, object>{{"alternate-exchange", "altexchange"}});
channel.ExchangeDeclare(exchange: "altexchange", type: ExchangeType.Fanout);


var message = "this is the msg.";

var body = Encoding.UTF8.GetBytes(message);

channel.BasicPublish("mainexchange", "test2", null, body);

Console.WriteLine($"Send msg: {message}");