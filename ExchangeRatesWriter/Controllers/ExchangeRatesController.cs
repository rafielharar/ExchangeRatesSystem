using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ExchangeRatesWriter.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExchangeRatesController : ControllerBase
    {
        public IExchangeRatesRepository m_ExchangeRatesRepository { get; set; }

        public ExchangeRatesController(IExchangeRatesRepository exchangeRepos)
        {
            m_ExchangeRatesRepository = exchangeRepos ?? throw new ArgumentException(nameof(exchangeRepos));
        }
        // GET: api/<ValuesController>
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(m_ExchangeRatesRepository.GetLatest());
        }
    }
}
