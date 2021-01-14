using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace ExchangeRatesWriter.Messaging
{
    public class EventBusRabbitMQConsumer: BackgroundService
    {
        public IConnection m_Connection { get; set; }
        private readonly IServiceScopeFactory m_ServiceScopeFactory;

        public EventBusRabbitMQConsumer(IConnection connection, IServiceScopeFactory serviceScopeFactory)
        {
            m_Connection = connection ?? throw new ArgumentException(nameof(IConnection));
            m_ServiceScopeFactory = serviceScopeFactory ?? throw new ArgumentException(nameof(serviceScopeFactory));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            consumeExchangeRatesChangeEvent();

            return Task.CompletedTask;
        }
     
        private void consumeExchangeRatesChangeEvent()
        {
            var exchangeName = QueueType.ExchangePairsUpdate.ToString();
            consumeAux(exchangeName, onRecieved);

            void onRecieved(object sender, string message)
            {
                var exchangeRates = JsonConvert.DeserializeObject<List<ExchangeRatesPair>>(message);
                getScopedExchangeRatesRepository().Save(exchangeRates);
            }
        }

        private void consumeAux(string exchangeName, EventHandler<string> onRecieved)
        {
            var channel = m_Connection.CreateModel();
            channel.ExchangeDeclare(exchangeName, ExchangeType.Fanout);
            string queueName = channel.QueueDeclare().QueueName;
            //durable: false, exclusive: false, autoDelete: false, arguments: null
            channel.QueueBind(queueName, exchangeName, routingKey: "");

            var consumer = new EventingBasicConsumer(channel);

            //Create event when something receive
            consumer.Received += recieved;

            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);


            async void recieved(object sender, BasicDeliverEventArgs e)
            {
                var message = Encoding.UTF8.GetString(e.Body.Span);
                Console.WriteLine(message);
                onRecieved(sender, message);
            }
        }

        private IExchangeRatesRepository getScopedExchangeRatesRepository()
        {
            var scope = m_ServiceScopeFactory.CreateScope();
            var services = scope.ServiceProvider;
            return services.GetRequiredService<IExchangeRatesRepository>();
        }
    }
}
