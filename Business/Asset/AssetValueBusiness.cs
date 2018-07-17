using Auctus.DataAccess.Asset;
using Auctus.DataAccess.Core;
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
            var lastUpdatedValue = LastAssetValue(asset.Id)?.Date ?? DateTime.UtcNow.AddDays(-30).Date;
            if (lastUpdatedValue.AddMinutes(ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES) > DateTime.UtcNow)
            {
                return;
            }
            Dictionary<DateTime, double> assetDateAndValues = ExchangeApi.GetCloseCryptoValue(asset.Code, lastUpdatedValue, MemoryCache);
            CreateAssetValueForPendingDates(asset, lastUpdatedValue, assetDateAndValues);
        }

        internal Dictionary<DateTime, List<AssetValue>> GetAssetValuesGroupedByDate(IEnumerable<int> assetsIds, DateTime startDate)
        {
            var assetValues = Data.List(assetsIds, startDate);
            return assetValues.GroupBy(av => av.Date).ToDictionary(av => av.Key, av => av.ToList());
        }

        internal IEnumerable<AssetValue> ListAssetValues(IEnumerable<int> assetsIds)
        {
            return Data.List(assetsIds);
        }

        private void CreateAssetValueForPendingDates(DomainObjects.Asset.Asset asset, DateTime lastUpdatedValue, Dictionary<DateTime, double> assetDateAndValues)
        {
            var pendingUpdate = assetDateAndValues?.Where(d => d.Key > lastUpdatedValue).OrderBy(v => v.Key);
            //var previousDateAndValue = assetDateAndValues?.Where(d => d.Key == lastUpdatedValue).OrderByDescending(v => v.Key).FirstOrDefault();
            if (pendingUpdate != null)
            {
                List<AssetValue> assetValues = new List<AssetValue>();
                foreach (var pending in pendingUpdate)
                {
                    //previousDateAndValue = InsertAssetValueForPreviousDaysWithoutMarketValues(asset, previousDateAndValue, pending);
                    assetValues.Add(new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = pending.Key, Value = pending.Value });
                }
                Data.InsertManyAsync(assetValues);
            }
        }

        private KeyValuePair<DateTime, double> InsertAssetValueForPreviousDaysWithoutMarketValues(DomainObjects.Asset.Asset asset, KeyValuePair<DateTime, double>? previousDateAndValue, KeyValuePair<DateTime, double> currentPendingDateAndValue)
        {
            if (previousDateAndValue.HasValue && previousDateAndValue?.Key > DateTime.MinValue)
            {
                var previousDate = previousDateAndValue?.Key.AddDays(1);
                List<AssetValue> previousAssetValues = new List<AssetValue>();

                while (previousDate < currentPendingDateAndValue.Key)
                {
                    previousAssetValues.Add(new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = previousDate.Value, Value = previousDateAndValue.Value.Value });
                    previousDate = previousDate.Value.AddDays(1);
                }
                Data.InsertManyAsync(previousAssetValues);
            }
            return currentPendingDateAndValue;
        }
    }
}
