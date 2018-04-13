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

        protected override string API_BASE_ENDPOINT { get => @"https://api.binance.com/"; }
        protected override string API_ENDPOINT { get => @"api/v1/aggTrades?symbol={0}{1}&endTime={2}&startTime={3}"; }
        protected override string BTC_SYMBOL { get => "BTC"; }
        protected override string USD_SYMBOL { get => "USDT"; }

        protected override string FormatRequestEndpoint(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            var queryStart = Util.Util.DatetimeToUnixMilliseconds(queryDate.AddHours(-1));
            var queryEnd = Util.Util.DatetimeToUnixMilliseconds(queryDate);
            return String.Format(API_ENDPOINT, fromSymbol, toSymbol, queryEnd, queryStart);
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BinanceApiResult[]>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
            return result?.Price;            
        }

        protected override ApiError GetErrorCode(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BinanceApiError>(response.Content.ReadAsStringAsync().Result);

            switch (result.code)
            {
                case "-1121":
                    return ApiError.InvalidSymbol;
                default:
                    return ApiError.UnknownError;
            }

        }

        public override List<ExchangeBalance> GetBalances()
        {
            throw new NotImplementedException();
        }
    }
}
