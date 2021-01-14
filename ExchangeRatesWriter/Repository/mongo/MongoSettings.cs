using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRatesWriter.Repository.mongo
{
    public class MongoSettings
    {
        public string ExchangeRatesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
