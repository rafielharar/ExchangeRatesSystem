using AutoMapper;
using Common;
using MongoRepository.Model;

namespace MongoRepository.Mapper
{
    public class ExchangeRatesPairMapper:Profile
    {
        public ExchangeRatesPairMapper()
        {
            CreateMap<ExchangeRatesPair, ExchangeRatesPairForMongo>();
            CreateMap<ExchangeRatesPairForMongo, ExchangeRatesPair>();
        }
    }
}
