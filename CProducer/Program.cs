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



channel.ExchangeDeclare(exchange: "headersexchange", type: ExchangeType.Headers);

var message = "This message will be sent with headers.";

var body = Encoding.UTF8.GetBytes(message);

var properties = channel.CreateBasicProperties();
properties.Headers = new Dictionary<string, object>
{
    { "name", "brian" }
};

channel.BasicPublish("headersexchange", "", properties, body);

Console.WriteLine($"Send msg: {message}");