using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;
using Auctus.DomainObjects.Portfolio;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

namespace Auctus.DataAccess.Exchanges
{
    public abstract class ExchangeApi
    {
        public const int GAP_IN_MINUTES_BETWEEN_VALUES = 30;
        protected readonly Cache MemoryCache;

        protected ExchangeApi(Cache cache = null)
        {
            MemoryCache = cache;
        }

        public static ExchangeApi GetById(int exchangeId, string apiKey, string apiSecretKey, Cache cache = null)
        {
            switch (exchangeId)
            {
                case 1:
                    return new BitfinexApi(apiKey, apiSecretKey, cache);
                case 2:
                    return new BinanceApi(cache);
                default:
                    throw new ArgumentException("Invalid exchange.");
            }
        }

        public static IEnumerable<ExchangeApi> GetApisByCode(Cache cache = null)
        {
            return new List<ExchangeApi> { new BinanceApi(cache) /*, new BitfinexApi(cache) */ };
        }

        protected enum ApiError
        {
            InvalidSymbol,
            UnknownError
        }
        protected class ApiErrorData
        {
            public ApiError apiError { get; set; }
            public string response { get; set; }
            public ApiErrorData(ApiError apiError, string response)
            {
                this.apiError = apiError;
                this.response = response;
            }
        }

        protected abstract string API_BASE_ENDPOINT { get; }
        protected abstract string API_ENDPOINT { get; }
        protected abstract string API_CURRENT_PRICE_ENDPOINT { get; }
        protected abstract string BTC_SYMBOL { get; }
        protected abstract string USD_SYMBOL { get; }
        protected abstract int DELAY_TO_CALL { get; }

        protected virtual Dictionary<DateTime, double> GetCloseAdjustedValues(DateTime startTime, string symbol)
        {
            var returnDictionary = new Dictionary<DateTime, double>();
            bool hasUSD;
            try { hasUSD = CallApiWithRetry(symbol, USD_SYMBOL, startTime).HasValue; } 
            catch { return returnDictionary; }

            var utcNow = DateTime.UtcNow;
            startTime = startTime.AddMinutes(GAP_IN_MINUTES_BETWEEN_VALUES);
            var calledCount = 0;
            while (startTime <= utcNow)
            {
                if (calledCount > 0 && calledCount % 10 == 0)
                    Thread.Sleep(DELAY_TO_CALL);

                var value = GetValueByDate(symbol, startTime, hasUSD);
                if (value != null && !returnDictionary.ContainsKey(startTime))
                    returnDictionary.Add(startTime, value.Value);
                else if (calledCount > 0 && returnDictionary.Count == 0)
                    return returnDictionary;

                startTime = startTime.AddMinutes(GAP_IN_MINUTES_BETWEEN_VALUES);
                ++calledCount;
            }
            return returnDictionary;
        }

        protected virtual Dictionary<string, double> GetCurrentValues(IEnumerable<string> symbols)
        {
            return CallCurrentValuesApi(symbols);
        }

        private double? GetValueByDate(string symbol, DateTime queryDate, bool hasUSD = false)
        {
            double? valueToReturn = null;

            try
            {
                if (hasUSD)
                {
                    valueToReturn = CallApiWithRetry(symbol, USD_SYMBOL, queryDate);
                }
                else
                {
                    double? symbolBtcValue = CallApiWithRetry(symbol, BTC_SYMBOL, queryDate);
                    double? btcUsdValue = CallApiWithRetry(BTC_SYMBOL, USD_SYMBOL, queryDate);

                    valueToReturn = symbolBtcValue * btcUsdValue;
                }
            }
            catch (Exception ex)
            {
                Email.SendErrorEmailAsync(string.Format("Exception GetValueByDate: symbol {0} ; Query Date: {1}", symbol, queryDate), ex);
            }

            return valueToReturn;
        }

        private double? CallApiWithRetry(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            return Retry.Get().Execute<double?>((Func<string, string, DateTime, double?>)CallApi, fromSymbol, toSymbol, queryDate);
        }

        private double? CallApi(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            string cacheKey = string.Format("{0}_{1}_{2}_{3}", API_BASE_ENDPOINT, fromSymbol, toSymbol, queryDate.Ticks);
            double? value = MemoryCache?.Get<double?>(cacheKey);
            if (value.HasValue)
                return value;
            else
            {
                using (var client = new HttpClient())
                {
                    ApiErrorData apiError;

                    client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                    var response = client.GetAsync(FormatRequestEndpoint(fromSymbol.Trim().ToUpper(), toSymbol.Trim().ToUpper(), queryDate)).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        value = GetCoinValue(response);
                        MemoryCache?.Set<double?>(cacheKey, value);
                        return value;
                    }
                    else
                        apiError = GetErrorCode(response);

                    switch (apiError.apiError)
                    {
                        case ApiError.InvalidSymbol:
                            return null;
                        case ApiError.UnknownError:
                        default:
                            throw new InvalidOperationException(apiError.response);
                    }
                }
            }
        }

        public static Dictionary<DateTime, double> GetCloseCryptoValue(string code, DateTime startDate, Cache cache = null)
        {
            var apis = GetApisByCode(cache);
            Dictionary<DateTime, List<double>> exchangesPrices = new Dictionary<DateTime, List<double>>();
            ConcurrentBag<Dictionary<DateTime, double>> apiResults = new ConcurrentBag<Dictionary<DateTime, double>>();
            Parallel.ForEach(apis, (api) =>
            {
                apiResults.Add(api.GetCloseAdjustedValues(startDate, code));
            });
            foreach (var result in apiResults)
            {
                foreach (var exchangeValue in result)
                {
                    if (!exchangesPrices.ContainsKey(exchangeValue.Key))
                        exchangesPrices.Add(exchangeValue.Key, new List<double>());

                    exchangesPrices[exchangeValue.Key].Add(exchangeValue.Value);
                }
            }
            return exchangesPrices.ToDictionary(v => v.Key, v => v.Value.Average());
        }

        private Dictionary<string, double> CallCurrentValuesApi(IEnumerable<string> symbols)
        {
            using (var client = new HttpClient())
            {
                ApiErrorData apiError;

                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(API_CURRENT_PRICE_ENDPOINT).Result;

                if (response.IsSuccessStatusCode)
                    return GetCurrentPriceValue(response, symbols);
                else
                    apiError = GetErrorCode(response);

                switch (apiError.apiError)
                {
                    case ApiError.InvalidSymbol:
                        return null;
                    case ApiError.UnknownError:
                    default:
                        throw new InvalidOperationException(apiError.response);
                }
            }
        }

        public static Dictionary<string, double> GetCurrentCryptoValues(IEnumerable<string> codes)
        {
            var apis = GetApisByCode();
            Dictionary<string, List<double>> exchangesPrices = new Dictionary<string, List<double>>();
            foreach (var api in apis)
            {
                var exchangeValues = api.GetCurrentValues(codes);
                foreach (var exchangeValue in exchangeValues)
                {
                    if (!exchangesPrices.ContainsKey(exchangeValue.Key))
                        exchangesPrices.Add(exchangeValue.Key, new List<double>());

                    exchangesPrices[exchangeValue.Key].Add(exchangeValue.Value);
                }
            }
            return exchangesPrices.ToDictionary(v => v.Key, v => v.Value.Average());
        }

        protected abstract string FormatRequestEndpoint(string fromSymbol, string toSymbol, DateTime queryDate);
        protected abstract double? GetCoinValue(HttpResponseMessage response);
        protected abstract Dictionary<string, double> GetCurrentPriceValue(HttpResponseMessage response, IEnumerable<string> symbols);
        protected abstract ApiErrorData GetErrorCode(HttpResponseMessage response);
        public abstract List<ExchangeBalance> GetBalances();
        public abstract void ValidateAccessPermissions();
    }
}

