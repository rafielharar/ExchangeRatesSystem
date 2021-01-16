using AutoMapper;
using Common;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoRepository.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MongoRepository
{
    public class ExchangeRatesMongoRepository : IExchangeRatesRepository
    {
        private IMongoCollection<ExchangeRatesPairForMongo> m_ExchangeRates;
        private readonly IMapper m_Mapper;

        public ExchangeRatesMongoRepository(IMongoSettings settings, IMapper mapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            m_ExchangeRates = database.GetCollection<ExchangeRatesPairForMongo>(settings.ExchangeRatesCollectionName);
            m_Mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        public void Save(List<ExchangeRatesPair> exchangeRatesPair)
        {
            //List<ExchangeRatesPairForMongo> exchanges =
            //    m_Mapper.Map<List<ExchangeRatesPairForMongo>>(exchangeRatesPair);
            //m_ExchangeRates.InsertMany(exchanges);

            foreach (var item in exchangeRatesPair)
            {
                m_ExchangeRates.InsertOne(new ExchangeRatesPairForMongo()
                {
                    BaseCurrency = item.BaseCurrency,
                    QuateCurrency = item.QuateCurrency,
                    Rate = item.Rate,
                    TimeStamp = item.TimeStamp
                });
            }
        }

        public IEnumerable<ExchangeRatesPair> GetLatest(Dictionary<string, string[]> currencyPairs)
        {
            List<ExchangeRatesPairForMongo> exchangeRates = new List<ExchangeRatesPairForMongo>();

            foreach (var item in currencyPairs)
            {
                string baseCurrency = item.Key;

                foreach (string quateCurrency in item.Value)
                {
                    var groupOfPairValues = m_ExchangeRates.AsQueryable().Where(
                        x => x.BaseCurrency == baseCurrency && x.QuateCurrency == quateCurrency);
                    var lastDateTime = groupOfPairValues.Max(r => r.TimeStamp);
                    var latestPair = groupOfPairValues.First(item => item.TimeStamp == lastDateTime);
                    exchangeRates.Add(latestPair);
                }
            }

            return m_Mapper.Map<List<ExchangeRatesPair>>(exchangeRates);
        }
    }
}
