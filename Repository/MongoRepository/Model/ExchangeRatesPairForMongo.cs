﻿using Common;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoRepository.Model
{
    [BsonIgnoreExtraElements]
    public class ExchangeRatesPairForMongo : ExchangeRatesPair
    {
    }
}
