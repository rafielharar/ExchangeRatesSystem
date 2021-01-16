# ExchangeRatesSystem

The 'ExchangeRatesReader' service shows the data for the following pairs: (as definded in appsettings.json)
USD/ILS
EUR/JPY
EUR/USD
EUR/GBP

According to test instructions, I was asked to show GBP/EUR instead of EUR/GBP, the reason I did this is because 'Live Rates' Api does not show data about that pair.

Usually, the exchange rate api does not update the data.

Url to request data: http://localhost:25670/api/ExchangeRates

To change the api, you need to go to ExchangeRatesReader project -> appsettings.json
and set to value of 'ApiType' to be one of the supported api:
1. LiveRatesApi
2. ExchangeRateApi

I decided to store the data in database over the file system, because of the following:
1. The read/write operations in db are faster
2. there is the option to run complex query on the data inside, insead of to read the entire data from file and execute query on the code.
3. better for scalability.

The times are presented in UTC fortmat.

The codes were written in vs2019.
