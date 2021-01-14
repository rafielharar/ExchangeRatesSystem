using System;
using System.Collections.Generic;

namespace Common
{
    public interface IExchangeRatesMapper
    {
        IEnumerable<ExchangeRatesPair> Map(string exchangeRates);
    }
}
