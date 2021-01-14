using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using ExchangeRatesReader.Messaging;
using Newtonsoft.Json;
using ExchangeRatesReader.Setting;

namespace ExchangeRatesReader.Service
{
    public class ExchangeRatesReaderService : BackgroundService
    {
        private readonly IServiceScopeFactory m_ServiceScopeFactory;
        private readonly ApiSettings m_ApiSettings;
        private readonly IEventBusProducer m_EventBusProducer;

        public ExchangeRatesReaderService(
            IServiceScopeFactory serviceScopeFactory, IConfiguration appConfig,
            IEventBusProducer eventBusProducer, ApiSettings apiSettings)
        {
            m_ServiceScopeFactory = serviceScopeFactory ?? throw new ArgumentException(nameof(serviceScopeFactory));
            m_EventBusProducer = eventBusProducer ?? throw new ArgumentException(nameof(eventBusProducer));
            m_ApiSettings = apiSettings ?? throw new ArgumentException(nameof(apiSettings));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            System.Timers.Timer timer = new System.Timers.Timer(m_ApiSettings.PollingInterval);

            timer.Elapsed += (arg1, arg2) =>
            {
                var exchangeRates = getExchangeRates();
                m_EventBusProducer.Send(exchangeRates);
                //File.AppendAllText("tst.txt", JsonConvert.SerializeObject(exchangeRates));
            };

            timer.Start();
            return Task.CompletedTask;
        }

        private List<ExchangeRatesPair> getExchangeRates()
        {
            List<ExchangeRatesPair> exchangePairs = new List<ExchangeRatesPair>();
            IExchangeRatesMapper exchangeRatesMapper = getExchangeRatesMapperService();

            foreach (string apiUrl in m_ApiSettings.Urls)
            {
                string exchangeRates = readDataAsync(apiUrl).Result;
                
                if(exchangeRates != null)
                {
                    exchangePairs.AddRange(exchangeRatesMapper.Map(exchangeRates));
                }
            }

            return exchangePairs;

            /* helpers */

            IExchangeRatesMapper getExchangeRatesMapperService()
            {
                var scope = m_ServiceScopeFactory.CreateScope();
                var services = scope.ServiceProvider;
                IExchangeRatesMapper service = services.GetRequiredService<IExchangeRatesMapper>();

                return service;
            }

            async Task<string> readDataAsync(string url)
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

                try
                {
                    using (HttpWebResponse response = (HttpWebResponse)await request.GetResponseAsync())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        return await reader.ReadToEndAsync();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
            }
        }

    }
}
