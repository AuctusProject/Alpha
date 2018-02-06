using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;

namespace Auctus.DataAccess.Exchanges
{
    public class BitfinexApi : ExchangeApi
    {
        private class BitfinexApiResult
        {
            [JsonProperty("price")]
            public double? Price { get; set; }
        }

        private class BitfinexApiError
        {
            [JsonProperty("message")]
            public string message { get; set; }
        }

        protected override string API_BASE_ENDPOINT { get => @"https://api.bitfinex.com/"; }
        protected override string API_ENDPOINT { get => @"v1/trades/{0}{1}?timestamp={2}&limit_trades=1"; }
        protected override string BTC_SYMBOL { get => "BTC"; }
        protected override string USD_SYMBOL { get => "USD"; }

        protected override string FormatRequestEndpoint(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            return String.Format(API_ENDPOINT, fromSymbol, toSymbol, Util.Util.DatetimeToUnixSeconds(queryDate));
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BitfinexApiResult[]>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
            return result?.Price;            
        }

        protected override ApiError GetErrorCode(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BitfinexApiError>(response.Content.ReadAsStringAsync().Result);

            switch (result.message)
            {
                case "Unknown symbol":
                    return ApiError.InvalidSymbol;
                default:
                    return ApiError.UnknownError;
            }
        }
    }
}
