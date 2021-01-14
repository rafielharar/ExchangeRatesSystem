using System;
using System.Collections.Generic;
using System.Text;
using Common;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace ExchangeRatesReader.Messaging
{
    public class RabbitMQEventBusProducer: IEventBusProducer
    {
        private IConnection m_Connection;
        
        public RabbitMQEventBusProducer(IConnection connection)
        {
            m_Connection = connection ?? throw new ArgumentException(nameof(IConnection));
        }

        public void Send(List<ExchangeRatesPair> exchangePairs)
        {
            var exchangeName = QueueType.ExchangePairsUpdate.ToString();
            var message = JsonConvert.SerializeObject(exchangePairs);

            sendAux(exchangeName, message);
        }

        private void sendAux(string exchange, string message)
        {
            using (var channel = m_Connection.CreateModel())
            {

                channel.ExchangeDeclare(exchange, ExchangeType.Fanout);

                var body = Encoding.UTF8.GetBytes(message);

                IBasicProperties properties = channel.CreateBasicProperties();

                channel.ConfirmSelect();
                channel.BasicPublish(exchange: exchange, routingKey: "", 
                    basicProperties: properties, body: body);

                channel.BasicAcks += (sender, eventArgs) =>
                {
                    Console.WriteLine("Sent RabbitMQ");
                };
                channel.ConfirmSelect();
            }
        }
    }
}
