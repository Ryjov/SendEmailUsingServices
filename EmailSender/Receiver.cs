using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailSender
{
    public class Receiver
    {
        public async Task ReceiveMessage()
        {
            ConnectionFactory factory = new();
            factory.Uri = new Uri(uriString: );
            factory.ClientProvidedName = "Rabbit sender app";

            IConnection cnn = factory.CreateConnection();
            IModel channel = cnn.CreateModel();

            string exchangeName = "DemoExchange";
            string routingKey = "demo-routing-key";
            string queueName = "DemoQueue";
        }
    }
}
