using Microsoft.AspNetCore.Connections;
using RabbitMQ.Client;
using System.Text;

namespace WebInterface
{
    public class ServiceMediator
    {
        private readonly IConfiguration Configuration;

        public void SendEmailToQueue(string text, string recipient)
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uriString: "amqp://guest:guest@localhost:5672");// add appsettings
            factory.ClientProvidedName = "Rabbit sender app";

            IConnection cnn = factory.CreateConnection();
            IModel channel = cnn.CreateModel();

            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";

            channel.ExchangeDeclare(exchangeName, ExchangeType.Direct);
            channel.QueueDeclare(queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queueName, exchangeName, routingKey, arguments: null);


            byte[] messageBodyBytes = Encoding.UTF8.GetBytes(s: text);
            channel.BasicPublish(exchangeName, routingKey, basicProperties: null, messageBodyBytes);

            channel.Close();
            cnn.Close();
        }
    }
}
