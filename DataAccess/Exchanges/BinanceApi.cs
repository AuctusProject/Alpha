using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;
using Auctus.DomainObjects.Portfolio;

namespace Auctus.DataAccess.Exchanges
{
    public class BinanceApi : ExchangeApi
    {
        private class BinanceApiResult
        {
            [JsonProperty("p")]
            public double Price { get; set; }
        }

        private class BinanceApiError
        {
            [JsonProperty("code")]
            public string code { get; set; }
            [JsonProperty("msg")]
            public string msg { get; set; }
        }

        private class BinanceCurrentPriceApiResult
        {
            [JsonProperty("symbol")]
            public string Symbol { get; set; }
            [JsonProperty("price")]
            public double Price { get; set; }
        }

        public BinanceApi(Cache cache = null) : base(cache) { }

        protected override string API_BASE_ENDPOINT { get => @"https://api.binance.com/"; }
        protected override string API_ENDPOINT { get => @"api/v1/aggTrades?symbol={0}{1}&endTime={2}&startTime={3}"; }
        protected override string API_CURRENT_PRICE_ENDPOINT { get => @"api/v3/ticker/price"; }
        protected override string BTC_SYMBOL { get => "BTC"; }
        protected override string USD_SYMBOL { get => "USDT"; }
        protected override int DELAY_TO_CALL { get => 100; }

        protected override string FormatRequestEndpoint(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            var queryStart = Util.Util.DatetimeToUnixMilliseconds(queryDate.AddMinutes(-Math.Min(GAP_IN_MINUTES_BETWEEN_VALUES, 60)));
            var queryEnd = Util.Util.DatetimeToUnixMilliseconds(queryDate);
            return String.Format(API_ENDPOINT, fromSymbol, toSymbol, queryEnd, queryStart);
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BinanceApiResult[]>(response.Content.ReadAsStringAsync().Result).LastOrDefault();
            return result?.Price;            
        }

        protected override Dictionary<string, double> GetCurrentPriceValue(HttpResponseMessage response, IEnumerable<string> symbols)
        {
            var result = JsonConvert.DeserializeObject<BinanceCurrentPriceApiResult[]>(response.Content.ReadAsStringAsync().Result);
            var currentPriceDictionary = result.ToDictionary(r => r.Symbol, r => r.Price);
            var returnDictionary = new Dictionary<string, double>();
            var btcCurrentPrice = currentPriceDictionary[BTC_SYMBOL + USD_SYMBOL];
            foreach (var symbol in symbols)
            {
                if (currentPriceDictionary.ContainsKey(symbol + USD_SYMBOL))
                {
                    returnDictionary.Add(symbol, currentPriceDictionary[symbol + USD_SYMBOL]);
                }
                else if (currentPriceDictionary.ContainsKey(symbol + BTC_SYMBOL))
                {
                    returnDictionary.Add(symbol, currentPriceDictionary[symbol + BTC_SYMBOL] * btcCurrentPrice);
                }
            }
            return returnDictionary;
        }

        protected override ApiErrorData GetErrorCode(HttpResponseMessage response)
        {
            string responseString = null;
            try
            {
                responseString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<BinanceApiError>(responseString);

                switch (result.code)
                {
                    case "-1121":
                        return new ApiErrorData(ApiError.InvalidSymbol, result.msg);
                    default:
                        return new ApiErrorData(ApiError.UnknownError, result.msg);
                }
            }
            catch
            {
                return new ApiErrorData(ApiError.UnknownError, responseString);
            }
        }

        public override List<ExchangeBalance> GetBalances()
        {
            throw new NotImplementedException();
        }

        public override void ValidateAccessPermissions()
        {
            throw new NotImplementedException();
        }
    }
}
