using AutoMapper;
using Common;
using ExchangeRatesWriter.Model;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRatesWriter.Repository.mongo
{
    public class ExchangeRatesRepository : IExchangeRatesRepository
    {
        private IMongoCollection<ExchangeRatesPairForMongo> m_ExchangeRates;
        private readonly IMapper m_Mapper;

        public ExchangeRatesRepository(MongoSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            m_ExchangeRates = database.GetCollection<ExchangeRatesPairForMongo>(settings.ExchangeRatesCollectionName);
            m_Mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }


        public void Save(List<ExchangeRatesPair> exchangeRatesPair)
        {
            List<ExchangeRatesPairForMongo> exchanges = m_Mapper.Map<List<ExchangeRatesPairForMongo>>(exchangeRatesPair);
            m_ExchangeRates.InsertMany(exchanges);
        }

        public IEnumerable<ExchangeRatesPair> GetLatest()
        {
            var lastDateTime = m_ExchangeRates.AsQueryable().Max(r => r.TimeStamp);
            var latest = m_ExchangeRates.AsQueryable().Where(item => item.TimeStamp == lastDateTime);
            return m_Mapper.Map<List<ExchangeRatesPair>>(latest);
        }
    }
}
