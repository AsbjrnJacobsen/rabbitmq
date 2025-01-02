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
channel.ExchangeDeclare(exchange: "dlx", type: ExchangeType.Fanout);


channel.QueueDeclare(queue: "mainexchangequeue", arguments: new Dictionary<string, object>
{
    {"x-dead-letter-exchange", "dlx"},
    {"x-message-ttl", 1000},
});
channel.QueueBind("mainexchangequeue", "mainexchange2", "test");

var mainConsumer = new EventingBasicConsumer(channel);
mainConsumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Main - Received msg: {message}");
};
channel.BasicConsume(queue: "mainexchangequeue", autoAck: true, consumer: mainConsumer);

channel.QueueDeclare(queue: "dlxexchangequeue");
channel.QueueBind("dlxexchangequeue", "dlx", "");

var dlxConsumer = new EventingBasicConsumer(channel);
dlxConsumer.Received += (model, ea) =>
{
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"dlx - Received msg: {message}");
};
channel.BasicConsume(queue: "dlxexchangequeue", consumer: dlxConsumer);

Console.WriteLine("Consuming...");
Console.ReadKey();