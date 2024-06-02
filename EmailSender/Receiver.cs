using Microsoft.Extensions.Configuration;
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

        public static async Task Main()
        {
            var emailService = new EmailService();
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");
            factory.ClientProvidedName = "Rabbit sender app";

            try
            {
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
                consumer.Received += (sender, args) =>
                {
                    var body = args.Body.ToArray();

                    string message = Encoding.UTF8.GetString(body);

                    // emailService.SendTextMail("bolt321@mail.ru", message);

                    channel.BasicAck(args.DeliveryTag, multiple: true);
                };

                string consumerTag = channel.BasicConsume(queueName, autoAck: false, consumer);

                channel.BasicCancel(consumerTag);

                channel.Close();
                cnn.Close();
            }
            catch (Exception ex)
            {
                string s = ex.Message;
            }
        }
    }
}
