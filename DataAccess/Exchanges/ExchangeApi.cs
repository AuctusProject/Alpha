using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Linq;
using Auctus.Util;
using Auctus.DomainObjects.Portfolio;

namespace Auctus.DataAccess.Exchanges
{
    public abstract class ExchangeApi
    {
        public static ExchangeApi GetById(int exchangeId, string apiKey, string apiSecretKey)
        {
            switch (exchangeId)
            {
                case 1:
                    return new BitfinexApi(apiKey, apiSecretKey);
                case 2:
                    return new BinanceApi();
                default:
                    throw new ArgumentException("Invalid exchange.");
            }
        }

        public static IEnumerable<ExchangeApi> GetApisByCode()
        {
            return new List<ExchangeApi> { new BinanceApi()/*, new BitfinexApi() */ };
        }

        protected enum ApiError
        {
            InvalidSymbol,
            UnknownError
        }

        protected abstract string API_BASE_ENDPOINT { get; }
        protected abstract string API_ENDPOINT { get; }
        protected abstract string API_CURRENT_PRICE_ENDPOINT { get; }
        protected abstract string BTC_SYMBOL { get; }
        protected abstract string USD_SYMBOL { get; }

        protected virtual Dictionary<DateTime, double> GetCloseAdjustedValues(DateTime date, string symbol)
        {
            var utcNow = DateTime.UtcNow.Date;
            var difference = utcNow - date;
            var daysToQuery = Math.Ceiling(difference.TotalDays);
            var returnDictionary = new Dictionary<DateTime, double>();

            bool hasUSD = CallApi(symbol, USD_SYMBOL, utcNow).HasValue;

            for (var i = daysToQuery; i > 0; i--)
            {
                var queryDate = utcNow.AddDays(-i);
                var value = GetValueByDate(symbol, queryDate, hasUSD);
                if (value != null && !returnDictionary.ContainsKey(queryDate))
                    returnDictionary.Add(queryDate, value.Value);
            }
            return returnDictionary;
        }

        protected virtual Dictionary<string, double> GetCurrentValues(IEnumerable<string> symbols)
        {
            return CallCurrentValuesApi(symbols);
        }

        private double? GetValueByDate(string symbol, DateTime queryDate, bool hasUSD = false)
        {
            double? valueToReturn;

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


            return valueToReturn;
        }

        private double? CallApiWithRetry(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            return Retry.Get().Execute<double?>((Func<string, string, DateTime, double?>)CallApi, fromSymbol, toSymbol, queryDate);
        }

        private double? CallApi(string fromSymbol, string toSymbol, DateTime queryDate)
        {
            using (var client = new HttpClient())
            {
                ApiError apiError;

                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(FormatRequestEndpoint(fromSymbol, toSymbol, queryDate)).Result;

                if (response.IsSuccessStatusCode)
                    return GetCoinValue(response);
                else
                    apiError = GetErrorCode(response);

                switch (apiError)
                {
                    case ApiError.InvalidSymbol:
                        return null;
                    case ApiError.UnknownError:
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public static Dictionary<DateTime, double> GetCloseCryptoValue(string code, DateTime startDate)
        {
            var apis = GetApisByCode();
            Dictionary<DateTime, List<double>> exchangesPrices = new Dictionary<DateTime, List<double>>();
            foreach (var api in apis)
            {
                var exchangeValues = api.GetCloseAdjustedValues(startDate, code);
                foreach (var exchangeValue in exchangeValues)
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
                ApiError apiError;

                client.BaseAddress = new Uri(API_BASE_ENDPOINT);
                var response = client.GetAsync(API_CURRENT_PRICE_ENDPOINT).Result;

                if (response.IsSuccessStatusCode)
                    return GetCurrentPriceValue(response, symbols);
                else
                    apiError = GetErrorCode(response);

                switch (apiError)
                {
                    case ApiError.InvalidSymbol:
                        return null;
                    case ApiError.UnknownError:
                    default:
                        throw new InvalidOperationException();
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
        protected abstract ApiError GetErrorCode(HttpResponseMessage response);
        public abstract List<ExchangeBalance> GetBalances();
        public abstract void ValidateAccessPermissions();
    }
}

