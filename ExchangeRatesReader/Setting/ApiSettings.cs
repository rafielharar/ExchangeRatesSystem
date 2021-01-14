using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExchangeRatesReader.Setting
{
    public class ApiSettings
    {
        public int PollingInterval { get; set; }
        public string[] Urls { get; set; }
    }
}
