using Auctus.Util.NotShared;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;

namespace Auctus.DataAccess.Asset
{
    public class AlphaVantageApi
    {
        private readonly static string AlphaVantageApiBaseUrl = "https://www.alphavantage.co/query";

        private class AlphaVantageApiResult
        {
            [JsonProperty("Time Series (Daily)")]
            public Dictionary<DateTime, AlphaVantageApiValuesResult> Values { get; set; }
        }

        private class AlphaVantageApiValuesResult
        {
            [JsonProperty("5. adjusted close")]
            public double AdjustedClose { get; set; }
        }

        private class AlphaVantageCryptoApiResult
        {
            [JsonProperty("Time Series (Digital Currency Daily)")]
            public Dictionary<DateTime, AlphaVantageCryptoApiValuesResult> Values { get; set; }
        }

        private class AlphaVantageCryptoApiValuesResult
        {
            [JsonProperty("4a. close (USD)")]
            public double Close { get; set; }
        }

        public static Dictionary<DateTime, double> GetCloseAdjustedValues(string symbol)
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(AlphaVantageApiBaseUrl);
                var response = client.GetAsync($"?function=TIME_SERIES_DAILY_ADJUSTED&outputsize=full&symbol={symbol}&apikey={Config.ALPHA_VANTAGE_API_KEY}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<AlphaVantageApiResult>(response.Content.ReadAsStringAsync().Result);
                    var returnDictionary = result.Values.ToDictionary(v =>v.Key, v =>v.Value.AdjustedClose);
                    return returnDictionary;
                }
                throw new InvalidOperationException();
            }
        }

        public static Dictionary<DateTime, double> GetCloseCryptoValue(string symbol)
        {
            using (HttpClient client = new HttpClient())
            {
                client.GetAsync(AlphaVantageApiBaseUrl);
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri(AlphaVantageApiBaseUrl);
                var response = client.GetAsync($"?function=DIGITAL_CURRENCY_DAILY&symbol={symbol}&market=USD&apikey={Config.ALPHA_VANTAGE_API_KEY}").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<AlphaVantageCryptoApiResult>(response.Content.ReadAsStringAsync().Result);
                    var returnDictionary = result.Values.ToDictionary(v => v.Key, v => v.Value.Close);
                    return returnDictionary;
                }
                throw new InvalidOperationException();
            }
        }
    }
}
