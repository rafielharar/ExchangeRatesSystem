using Common;
using System;

namespace Common
{
    public interface IMongoSettings
    {
        public string ExchangeRatesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
