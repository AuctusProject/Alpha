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
using Auctus.DomainObjects.Portfolio;

namespace Auctus.DataAccess.Exchanges
{
    public class BitfinexApi : ExchangeApi
    {
        private string apiKey;
        private string apiSecretKey;

        public BitfinexApi(Cache cache = null) : base(cache) { }

        public BitfinexApi(string apiKey, string apiSecretKey, Cache cache = null) : base(cache)
        {
            this.apiKey = apiKey;
            this.apiSecretKey = apiSecretKey;
        }

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

        public class BitfinexKeyInfo
        {
            [JsonProperty("wallets")]
            public BitfinexReadWritePermission Wallets { get; set; }
        }

        public class BitfinexReadWritePermission
        {
            [JsonProperty("read")]
            public bool? Read { get; set; }
            [JsonProperty("write")]
            public bool? Write{ get; set; }
        }

        protected override string API_BASE_ENDPOINT { get => @"https://api.bitfinex.com/"; }
        protected override string API_ENDPOINT { get => @"v2/trades/t{0}{1}/hist?start={2}&end={3}"; }
        protected override string API_CURRENT_PRICE_ENDPOINT { get => @"api/v3/ticker/price"; }
        protected override string BTC_SYMBOL { get => "BTC"; }
        protected override string USD_SYMBOL { get => "USD"; }
        protected override int DELAY_TO_CALL { get => 59000; }

        protected override string FormatRequestEndpoint(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            var queryStart = Util.Util.DatetimeToUnixMilliseconds(queryDate.AddMinutes(-Math.Min(GAP_IN_MINUTES_BETWEEN_VALUES, 60)));
            var queryEnd = Util.Util.DatetimeToUnixMilliseconds(queryDate);
            return String.Format(API_ENDPOINT, fromSymbol, toSymbol, queryStart, queryEnd);
        }

        protected override double? GetCoinValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<double[][]>(response.Content.ReadAsStringAsync().Result).FirstOrDefault();
            return result?[3];            
        }

        protected override Dictionary<string, double> GetCurrentPriceValue(HttpResponseMessage response, IEnumerable<string> symbols)
        {
            throw new NotImplementedException();
        }

        protected override ApiErrorData GetErrorCode(HttpResponseMessage response)
        {
            string responseString = null;
            try
            {
                responseString = response.Content.ReadAsStringAsync().Result;
                var result = JsonConvert.DeserializeObject<BitfinexApiError>(responseString);

                switch (result.message)
                {
                    case "Unknown symbol":
                        return new ApiErrorData(ApiError.InvalidSymbol, result.message);
                    default:
                        return new ApiErrorData(ApiError.UnknownError, result.message);
                }
            }
            catch
            {
                return new ApiErrorData(ApiError.UnknownError, responseString);
            }
        }

        protected static string GetHMAC384(string message, string apiSecret)
        {
            var hmac = new HMACSHA384(Encoding.ASCII.GetBytes(apiSecret));
            byte[] hashmessage = hmac.ComputeHash(Encoding.ASCII.GetBytes(message));
            var sign = BitConverter.ToString(hashmessage).Replace("-", "").ToLower();
            return sign;
        }

        private string PostAutenticatedRequest(String path)
        {
            string urlBase = "https://api.ethfinex.com";
            string urlCompleta = urlBase + path;
            BitfinexPostBase bodyPost = new BitfinexPostBase();
            bodyPost.Request = path;
            bodyPost.Nonce = DateTime.UtcNow.Ticks.ToString();
            var jsonObj = JsonConvert.SerializeObject(bodyPost);
            var payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonObj));

            try
            {
                using (var client = new WebClient())
                {
                    client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                    client.Headers["X-BFX-APIKEY"] = apiKey;
                    client.Headers["X-BFX-PAYLOAD"] = payload;
                    client.Headers["X-BFX-SIGNATURE"] = GetHMAC384(payload, apiSecretKey);
                    return client.UploadString(urlCompleta, "POST", jsonObj);
                }
            }
            catch(WebException e)
            {
                var response = e.Response as System.Net.HttpWebResponse;
                if (response?.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new ArgumentException("Invalid authentication keys");
                }
                throw;
            }
        }

        public override List<ExchangeBalance> GetBalances()
        {
            var requestPath = "/v1/balances";
            string result = PostAutenticatedRequest(requestPath);
            var balances = JsonConvert.DeserializeObject<BitfinexBalance[]>(result);
            var positiveBalances = balances.Where(b => b.Amount > 0.0).GroupBy(g => g.Currency).Select(c => new ExchangeBalance()
            {
                CurrencyCode = c.Key,
                Amount = c.Sum(s => s.Amount)
            }).ToList();
            return positiveBalances;
        }

        public override void ValidateAccessPermissions()
        {
            var requestPath = "/v1/key_info";
            string result = PostAutenticatedRequest(requestPath);
            var keyInfo = JsonConvert.DeserializeObject<BitfinexKeyInfo>(result);
            if (keyInfo?.Wallets?.Read != true)
            {
                throw new ArgumentException("Invalid authentication keys");
            }
        }
    }
}
