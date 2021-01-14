using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public interface IEventBusProducer
    {
        void Send(List<ExchangeRatesPair> exchangePairs);
    }
}
