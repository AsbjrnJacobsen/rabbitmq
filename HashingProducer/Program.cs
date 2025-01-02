using System.Text;
using RabbitMQ.Client;

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


for (var i = 0; i < 1000; i++)
{
    var message = $"Hello hash the routing and pass me on please. {i}";

    var body = Encoding.UTF8.GetBytes(message);

    var routingKey = $"this will be hashed, but how does it work exactly? {i}";


    channel.BasicPublish("simplehashing", routingKey, null, body);
    Console.WriteLine($"Send msg: {message}");
}