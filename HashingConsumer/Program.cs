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

channel.ExchangeDeclare("simplehashing", "x-consistent-hash");

channel.QueueDeclare(queue: "letterbox1");
channel.QueueDeclare(queue: "letterbox2");
channel.QueueDeclare(queue: "letterbox3");


channel.QueueBind("letterbox1", "simplehashing", "1"); //25%
channel.QueueBind("letterbox2", "simplehashing", "2"); //75%
channel.QueueBind("letterbox3", "simplehashing", "3"); //75%

int que1 = 0, que2 = 0, que3 = 0;


var consumer1 = new EventingBasicConsumer(channel);
consumer1.Received += (model, ea) =>
{
    que1++;
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue1 - Received msg: {message}");
};
channel.BasicConsume(queue: "letterbox1", autoAck: true, consumer: consumer1);

var consumer2 = new EventingBasicConsumer(channel);
consumer2.Received += (model, ea) =>
{
    que2++;
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue 2 - Received msg: {message}");
};
channel.BasicConsume(queue: "letterbox2", autoAck: true, consumer: consumer2);

var consumer3 = new EventingBasicConsumer(channel);
consumer3.Received += (model, ea) =>
{
    que3 ++;
    var body = ea.Body.ToArray();
    var message = Encoding.UTF8.GetString(body);
    Console.WriteLine($"Queue  3 - Received msg: {message}");
};
channel.BasicConsume(queue: "letterbox3", autoAck: true, consumer: consumer3);

// Baggrundstråd til overvågning af tastetryk
await Task.Run(() =>
{
    while (true)
    {
        if (Console.KeyAvailable)
        {
            var key = Console.ReadKey(intercept: true).Key;
            if (key == ConsoleKey.S)
            {
                Console.WriteLine($"\nQueues called: 1: {que1}, 2: {que2}, 3: {que3}");
            }

            if (key == ConsoleKey.A)
            {
                que1 = 0;
                que2 = 0;
                que3 = 0;
            }
            else if (key == ConsoleKey.Q)
            {
                Console.WriteLine("\nExiting...");
                Environment.Exit(0); // Afslut programmet
            }
        }

        Thread.Sleep(100); // Undgå CPU-overforbrug
    }
});

Console.WriteLine("Consuming... Press 'S' to show queue stats, 'Q' to quit.");
Console.ReadLine(); // Hold hovedtråden kørende