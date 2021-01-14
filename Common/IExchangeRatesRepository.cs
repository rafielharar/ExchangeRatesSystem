using System;
using System.Collections.Generic;
using System.Text;

namespace Common
{
    public interface IExchangeRatesRepository
    {
        void Save(List<ExchangeRatesPair> exchanges);
        IEnumerable<ExchangeRatesPair> GetLatest(Dictionary<string, string[]> currencyPairs);
    }
}
