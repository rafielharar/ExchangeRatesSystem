using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class QueueType
    {
        private string _type;

        private QueueType(string type)
        {
            _type = type;
        }

        public override string ToString()
        {
            return _type;
        }

        public static QueueType ExchangePairsUpdate = new QueueType("Exchange-Pairs-Update");
    }

}
