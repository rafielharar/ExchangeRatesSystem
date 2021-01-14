using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExchangeRatesAPI
{
    public class ExchangeRatesMapper : IExchangeRatesMapper
    {
        public IEnumerable<ExchangeRatesPair> Map(string data)
        {
            List<ExchangeRatesPair> exchangePairs = new List<ExchangeRatesPair>();

            ExchangeRatesExchangeRatesPair liveRatesExchangePair =
                JsonConvert.DeserializeObject<ExchangeRatesExchangeRatesPair>(data);

            foreach (var quateAndRatePair in liveRatesExchangePair.rates)
            {
                exchangePairs.Add(new ExchangeRatesPair()
                {
                    BaseCurrency = liveRatesExchangePair.baseCurrency,
                    QuateCurrency = quateAndRatePair.Key,
                    Rate = quateAndRatePair.Value,

                    TimeStamp = liveRatesExchangePair.date
                });
            }


            return exchangePairs;
        }

    }
}
