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

channel.ExchangeDeclare(exchange: "mytopicexchange", ExchangeType.Topic);

var queName = channel.QueueDeclare().QueueName;

channel.QueueBind(queue: queName, exchange: "mytopicexchange", routingKey: "user.#");

var consumer = new EventingBasicConsumer(channel);


consumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"User - Received msg: {message}");
};

channel.BasicConsume(queue: queName, autoAck: true, consumer: consumer);

Console.WriteLine("User - Consuming...");
Console.ReadKey();