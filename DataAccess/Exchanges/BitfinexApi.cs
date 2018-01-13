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
        
        protected override string API_BASE_ENDPOINT { get => @"https://api.bitfinex.com/"; }
        protected override string API_ENDPOINT { get => @"v1/trades/{0}USD?timestamp={1}&limit_trades=1"; }

        protected override string FormatRequestEndpoint(string symbol, DateTime queryDate)
        {
            return String.Format(API_ENDPOINT, symbol, Util.Util.DatetimeToUnixSeconds(queryDate));
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BitfinexApiResult[]>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
            return result?.Price;            
        }
    }
}
