﻿using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Receiver
    {
        private readonly IConfiguration Configuration;

        public async Task ReceiveMessage()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uriString: Configuration["RabbitMQ:UriString"]);
            factory.ClientProvidedName = "Rabbit sender app";

            IConnection cnn = factory.CreateConnection();
            IModel channel = cnn.CreateModel();

            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);
            channel.BasicQos(prefetchSize: 0, prefetchCount: 1, global: false);

            var consumer = new EventingBasicConsumer(channel);
        }
    }
}
