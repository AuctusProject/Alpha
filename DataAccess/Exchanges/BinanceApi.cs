using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;

namespace Auctus.DataAccess.Exchanges
{
    public class BinanceApi : ExchangeApi
    {
        private class BinanceApiResult
        {
            [JsonProperty("p")]
            public double Price { get; set; }
        }
        
        protected override string API_BASE_ENDPOINT { get => @"https://api.binance.com/"; }
        protected override string API_ENDPOINT { get => @"api/v1/aggTrades?symbol={0}USDT&endTime={1}&startTime={2}"; }

        protected override string FormatRequestEndpoint(string symbol, DateTime queryDate)
        {
            var queryStart = Util.Util.DatetimeToUnixMilliseconds(queryDate.AddHours(-1));
            var queryEnd = Util.Util.DatetimeToUnixMilliseconds(queryDate);
            return String.Format(API_ENDPOINT, symbol, queryEnd, queryStart);
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<BinanceApiResult[]>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
            return result?.Price;            
        }
    }
}
