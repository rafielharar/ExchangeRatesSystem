using AutoMapper;
using Common;
using ExchangeRatesWriter.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRatesWriter.Mapper
{
    public class ExchangeRatesPairMapper:Profile
    {
        public ExchangeRatesPairMapper()
        {
            CreateMap<ExchangeRatesPair, ExchangeRatesPairForMongo>().ReverseMap();
        }
    }
}
