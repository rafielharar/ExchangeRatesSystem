using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;

namespace LiveRatesAPI
{
    public class ExchangeRatesMapper : IExchangeRatesMapper
    {
        public IEnumerable<ExchangeRatesPair> Map(string data)
        {
            List<ExchangeRatesPair> exchangePairs = new List<ExchangeRatesPair>();
            List<LiveRatesExchangeRatesPair> liveRatesExchangePair =
                JsonConvert.DeserializeObject<List<LiveRatesExchangeRatesPair>>(data);

            foreach (var item in liveRatesExchangePair)
            {
                string[] currencies = item.currency.Split('/');
                DateTime formattedDt = new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(item.timestamp);

                exchangePairs.Add(new ExchangeRatesPair()
                {
                    BaseCurrency = currencies[0],
                    QuateCurrency = currencies[1],
                    Rate = item.rate,
                    TimeStamp = formattedDt
                });
            }

            return exchangePairs;
        }
    }
}
