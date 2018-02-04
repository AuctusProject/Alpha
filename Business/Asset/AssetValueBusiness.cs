using Auctus.DataAccess.Asset;
using Auctus.DataAccess.Exchanges;
using Auctus.DomainObjects.Asset;
using Auctus.Util;
using Dapper;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.Business.Asset
{
    public class AssetValueBusiness : BaseBusiness<AssetValue, AssetValueData>
    {
        public AssetValueBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        internal AssetValue LastAssetValue(int assetId)
        {
            return Data.GetLastValue(assetId);
        }

        internal void UpdateAssetValue(DomainObjects.Asset.Asset asset)
        {
            var lastUpdatedValue = LastAssetValue(asset.Id)?.Date ?? new DateTime(2015, 8, 8);
            if (lastUpdatedValue >= DateTime.UtcNow.Date)
            {
                return;
            }
            Dictionary<DateTime, double> assetDateAndValues = GetAssetValuesByDate(asset, lastUpdatedValue);
            CreateAssetValueForPendingDates(asset, lastUpdatedValue, assetDateAndValues);
        }

        internal Dictionary<DateTime, List<AssetValue>> GetAssetValuesGroupedByDate(IEnumerable<int> assetsIds, DateTime startDate)
        {
            var assetValues = Data.List(assetsIds, startDate);

            return assetValues.GroupBy(av => av.Date).ToDictionary(av => av.Key, av => av.ToList());

        }

        private void CreateAssetValueForPendingDates(DomainObjects.Asset.Asset asset, DateTime lastUpdatedValue, Dictionary<DateTime, double> assetDateAndValues)
        {
            var pendingUpdate = assetDateAndValues?.Where(d => d.Key > lastUpdatedValue).OrderBy(v => v.Key);
            var previousDateAndValue = assetDateAndValues?.Where(d => d.Key == lastUpdatedValue).OrderByDescending(v => v.Key).FirstOrDefault();
            if (pendingUpdate != null)
            {
                foreach (var pending in pendingUpdate)
                {
                    previousDateAndValue = InsertAssetValueForPreviousDaysWithoutMarketValues(asset, previousDateAndValue, pending);
                    var assetValue = new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = pending.Key, Value = pending.Value };
                    Data.Insert(assetValue);
                }
            }
        }

        private KeyValuePair<DateTime, double> InsertAssetValueForPreviousDaysWithoutMarketValues(DomainObjects.Asset.Asset asset, KeyValuePair<DateTime, double>? previousDateAndValue, KeyValuePair<DateTime, double> currentPendingDateAndValue)
        {
            if (previousDateAndValue.HasValue && previousDateAndValue?.Key > DateTime.MinValue)
            {
                var previousDate = previousDateAndValue?.Key.AddDays(1);
                while (previousDate < currentPendingDateAndValue.Key)
                {
                    var previousAssetValue = new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = previousDate.Value, Value = previousDateAndValue.Value.Value };
                    Data.Insert(previousAssetValue);
                    previousDate = previousDate.Value.AddDays(1);
                }
            }
            return currentPendingDateAndValue;
        }

        private static Dictionary<DateTime, double> GetAssetValuesByDate(DomainObjects.Asset.Asset asset, DateTime startDate)
        {
            Dictionary<DateTime, double> assetDateAndValues;
            if (asset.Type == DomainObjects.Asset.AssetType.Traditional)
            {
                assetDateAndValues = AlphaVantageApi.GetCloseAdjustedValues(asset.Code);
            }
            else if (asset.Type == DomainObjects.Asset.AssetType.Crypto)
            {
                assetDateAndValues = ExchangeApi.GetCloseCryptoValue(asset.Code, startDate);
            }
            else
                throw new InvalidOperationException();
            return assetDateAndValues;
        }
    }
}
