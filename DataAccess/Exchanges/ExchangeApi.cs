using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;

namespace Auctus.DataAccess.Exchanges
{
    public abstract class ExchangeApi
    {
        public static IEnumerable<ExchangeApi> GetApisByCode(string coinSymbol)
        {
            switch (coinSymbol)
            {
                case "BTC":
                case "ETH":
                    return new List<ExchangeApi> { new BinanceApi()/*, new BitfinexApi() */ };
                default:
                    return new List<ExchangeApi>();
            }
        }

        protected abstract string API_BASE_ENDPOINT { get; }
        protected abstract string API_ENDPOINT { get; }

        protected virtual Dictionary<DateTime, double> GetCloseAdjustedValues(DateTime date, string symbol)
        {
            var utcNow = DateTime.UtcNow.Date;
            var difference = utcNow - date;
            var daysToQuery = Math.Ceiling(difference.TotalDays);
            var returnDictionary = new Dictionary<DateTime, double>();
            for (var i = daysToQuery; i > 0; i--)
            {
                var queryDate = utcNow.AddDays(-i);
                var value = CallApiWithRetry(symbol, queryDate);
                if (value != null && !returnDictionary.ContainsKey(queryDate))
                    returnDictionary.Add(queryDate, value.Value);
            }
            return returnDictionary;
        }

        private double? CallApiWithRetry(string symbol, DateTime queryDate)
        {
            return Retry.Get().Execute<double?>((Func<string, DateTime, double?>)CallApi, symbol, queryDate);
        }

        private double? CallApi(string symbol, DateTime queryDate)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(FormatRequestEndpoint(symbol, queryDate)).Result;
                if (response.IsSuccessStatusCode)
                {
                    return GetCoinValue(response);
                }
                throw new InvalidOperationException();
            }
        }

        protected abstract string FormatRequestEndpoint(string symbol, DateTime queryDate);
        protected abstract double? GetCoinValue(HttpResponseMessage response);

        public static Dictionary<DateTime, double> GetCloseCryptoValue(string code, DateTime startDate)
        {
            var apis = GetApisByCode(code);
            Dictionary<DateTime, List<double>> exchangesPrices = new Dictionary<DateTime, List<double>>();
            foreach (var api in apis)
            {
                var exchangeValues = api.GetCloseAdjustedValues(startDate, code);
                foreach(var exchangeValue in exchangeValues)
                {
                    if (!exchangesPrices.ContainsKey(exchangeValue.Key))
                        exchangesPrices.Add(exchangeValue.Key, new List<double>());

                    exchangesPrices[exchangeValue.Key].Add(exchangeValue.Value);
                }
            }
            return exchangesPrices.ToDictionary(v => v.Key, v => v.Value.Average());
        }
    }
}

