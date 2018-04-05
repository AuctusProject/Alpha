using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;
using SimpleJson;
using System.Security.Cryptography;
using System.Text;
using System.Net;

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

        public class BitfinexPostBase
        {
            [JsonProperty("request")]
            public string Request { get; set; }

            [JsonProperty("nonce")]
            public string Nonce { get; set; }
        }

        public class BitfinexBalance
        {
            [JsonProperty("type")]
            public string Type { get; set; }
            [JsonProperty("currency")]
            public string Currency { get; set; }
            [JsonProperty("amount")]
            public double Amount { get; set; }
            [JsonProperty("available")]
            public double Available { get; set; }
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

        protected static string ObterHMAC384(string message, string apiSecret)
        {
            var hmac = new HMACSHA384(Encoding.ASCII.GetBytes(apiSecret));
            byte[] hashmessage = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
            var sign = BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            return sign;
        }

        public static string GetBalances()
        {
            var apiKey = "";
            var apiSecret = "";
            var requestPath = "/v1/balances";
            string urlBase = "https://api.ethfinex.com";
            string urlCompleta = urlBase + requestPath;
            BitfinexPostBase bodyPost = new BitfinexPostBase();
            bodyPost.Request = requestPath;
            bodyPost.Nonce = DateTime.UtcNow.Ticks.ToString();
            var jsonObj = JsonConvert.SerializeObject(bodyPost);
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonObj));
            string result;
            using (var client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers["X-BFX-APIKEY"] = apiKey;
                client.Headers["X-BFX-PAYLOAD"] = payload;
                client.Headers["X-BFX-SIGNATURE"] = ObterHMAC384(payload, apiSecret);
                result = client.UploadString(urlCompleta, "POST", jsonObj);
            }
            var balances = JsonConvert.DeserializeObject<BitfinexBalance[]>(result);
            var positiveBalances = balances.Where(b => b.Amount > 0.0).GroupBy(g => g.Currency).Select(c => new BitfinexBalance()
            {
                Currency = c.Key,
                Amount=c.Sum(s => s.Amount)
            }).ToList();
            return "";
        }
    }
}
