using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ExchangeRatesAPI
{
    public class ExchangeRatesExchangeRatesPair
    {
        public Dictionary<string, float> rates{ get; set; }

        [JsonProperty("base")]
        public string baseCurrency { get; set; }
        public DateTime date { get; set; }
    }
}
