using Common;
using System;

namespace LiveRatesAPI
{
    public class LiveRatesExchangeRatesPair
    {
        public string currency { get; set; }
        public float rate { get; set; }
        public long timestamp { get; set; }
    }
}
