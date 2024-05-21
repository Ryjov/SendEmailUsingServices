using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;

namespace WebInterface
{
    public class ServiceMediator
    {
        private readonly IConfiguration Configuration;

        public void SendEnailToQueue()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uriString: Configuration["RabbitMQ:UriString"]);// add appsettings
            factory.ClientProvidedName = "Rabbit sender app";

            IConnection cnn = factory.CreateConnection();
            IModel channel = cnn.CreateModel();

            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);

            for (int i = 0; i < 60; i++)
            {
                byte[] messageBodyBytes = Encoding.UTF8.GetBytes(s: $"Message #{i}");
                channel.BasicPublish(exchangeName, routingKey, basicProperties: null, messageBodyBytes);
                Thread.Sleep(1000);
            }

            channel.Close();
            cnn.Close();
        }
    }
}
