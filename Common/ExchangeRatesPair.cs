using Common;
using System;

namespace Common
{
    public class ExchangeRatesPair
    {
        public string BaseCurrency { get; set; }
        public string QuateCurrency { get; set; }
        public float Rate { get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
