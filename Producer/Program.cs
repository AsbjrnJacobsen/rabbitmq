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

channel.ExchangeDeclare(exchange: "mytopicexchange", type: ExchangeType.Topic);

var userPaymentsMessage = "A european user paid for something.";

var userPaymentbody = Encoding.UTF8.GetBytes(userPaymentsMessage);

channel.BasicPublish("mytopicexchange", "user.europe.payments", null, userPaymentbody);

Console.WriteLine($"Send msg: {userPaymentbody}");


var businessPaymentsMessage = "A european business paid for something.";

var businessPaymentbody = Encoding.UTF8.GetBytes(businessPaymentsMessage);

channel.BasicPublish("mytopicexchange", "business.europe.payments", null, businessPaymentbody);

Console.WriteLine($"Send msg: {businessPaymentbody}");