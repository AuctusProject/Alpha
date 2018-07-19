using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;
using Auctus.DomainObjects.Portfolio;

namespace Auctus.DataAccess.Exchanges
{
    public class CoinMarketCapApi
    {
        protected string API_BASE_ENDPOINT { get => @"https://api.coinmarketcap.com/"; }
        protected string API_ENDPOINT { get => @"v2/ticker/?limit=100&sort=id&structure=array&start={0}"; }
        protected string LISTINGS_ENDPOINT { get => @"v2/listings/"; }

        public class CoinMarketCapAssetsResult
        {
            [JsonProperty("data")]
            public CoinMarketCapAssetResult[] Data { get; set; }
        }

        public class CoinMarketCapAssetResult
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("symbol")]
            public string Symbol { get; set; }
        }

        private class CoinMarketCapResult
        {
            [JsonProperty("data")]
            public CoinMarketCapDataResult[] Data { get; set; }
            
        }

        private class CoinMarketCapDataResult
        {
            [JsonProperty("id")]
            public int Id { get; set; }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("symbol")]
            public string Symbol { get; set; }
            [JsonProperty("quotes")]
            public CoinMarketCapQuoteResult Quotes{ get; set; }

        }

        private class CoinMarketCapQuoteResult
        {
            [JsonProperty("USD")]
            public CoinMarketCapPriceResult USD { get; set; }
        }

        private class CoinMarketCapPriceResult
        {
            [JsonProperty("price")]
            public double? Price { get; set; }
        }

        private Dictionary<int, double> GetAllCurrentPriceValue(HttpResponseMessage response)
        {
            var result = JsonConvert.DeserializeObject<CoinMarketCapResult>(response.Content.ReadAsStringAsync().Result);
            return result?.Data?.Where(d => d.Quotes.USD.Price.HasValue).ToDictionary(r => r.Id, r => r.Quotes.USD.Price.Value);
        }

        public Dictionary<int, double> GetAllCoinsCurrentPrice()
        {
            var currentPage = 0;
            var returnDictionary = new Dictionary<int, double>();
            while (true)
            {
                var dictionary = CallApiTickerWithRetry(currentPage * 100 + 1);
                if (dictionary?.Any() == true)
                    returnDictionary = returnDictionary.Concat(dictionary).ToDictionary(x => x.Key, x => x.Value);
                else
                    return returnDictionary;

                currentPage++;
            }
        }

        private Dictionary<int, double> CallApiTickerWithRetry(int start)
        {
            return Retry.Get().Execute<Dictionary<int, double>>((Func<int, Dictionary<int, double>>)CallApiTicker, start);
        }

        private Dictionary<int, double> CallApiTicker(int start)
        {
            var returnDictionary = new Dictionary<int, double>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(String.Format(API_ENDPOINT, start)).Result;

                if (response.IsSuccessStatusCode || response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    var dictionary = GetAllCurrentPriceValue(response);
                    if (dictionary?.Any() == true)
                        returnDictionary = returnDictionary.Concat(dictionary).ToDictionary(x => x.Key, x => x.Value);
                    else
                        return returnDictionary;
                }
                else
                    throw new InvalidOperationException();
            }
            return returnDictionary;
        }

        public CoinMarketCapAssetResult[] GetAllCoinMarketCapAssets()
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(LISTINGS_ENDPOINT).Result;

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<CoinMarketCapAssetsResult>(response.Content.ReadAsStringAsync().Result);
                    return result?.Data;
                }
                else
                    throw new InvalidOperationException();
            }
        }
    }
}
