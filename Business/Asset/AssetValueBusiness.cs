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

        public void UpdateAllAssetsValues()
        {
            var assets = AssetBusiness.ListAssets();
            var currentValuesDictionary = new CoinMarketCapApi().GetAllCoinsCurrentPrice();
            var currentDate = DateTime.UtcNow;
            currentDate = currentDate.AddMilliseconds(-currentDate.Millisecond);
            var assetValues = new List<DomainObjects.Asset.AssetValue>();

            foreach (var currentValue in currentValuesDictionary)
            {
                var asset = assets.FirstOrDefault(a => a.CoinMarketCapId == currentValue.Key);
                if (asset != null)
                    assetValues.Add(new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = currentDate, Value = currentValue.Value });
            }
            Data.InsertManyAsync(assetValues);
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
            var now = DateTime.UtcNow;
            var rangeValueInMinutes = GetRangeValueToGroupAssetsInMinutes((int)Math.Ceiling(now.Subtract(startDate).TotalMinutes));
            var iterateDate = startDate.AddSeconds(-startDate.Second).AddMilliseconds(-startDate.Millisecond);
            var passedMinutes = (iterateDate.Hour * 60 + iterateDate.Minute);
            iterateDate = iterateDate.AddMinutes(rangeValueInMinutes).AddMinutes(-(passedMinutes % rangeValueInMinutes));
            var result = new Dictionary<DateTime, List<AssetValue>>();
            while (iterateDate < now)
            {
                var minimumDate = iterateDate.AddMinutes(-Math.Min((rangeValueInMinutes * 6), 1440));
                var assets = assetValues.Where(c => c.Date > minimumDate && c.Date <= iterateDate).GroupBy(c => c.AssetId).Select(s => s.OrderByDescending(x => x.Date).FirstOrDefault()).Where(c => c != null);
                result[iterateDate] = new List<AssetValue>();
                result[iterateDate].AddRange(assets);
                iterateDate = iterateDate.AddMinutes(rangeValueInMinutes);
            }
            return result;
        }

        private int GetRangeValueToGroupAssetsInMinutes(int totalMinutes)
        {
            var expected = totalMinutes / 300;
            if (expected <= 5)
                return 5;
            var possibilities = new int[] { 5, 10, 15, 20, 30, 45, 60, 90, 120, 180, 240, 360, 480, 720, 1440 };
            return possibilities.Where(c => c <= expected).OrderByDescending(c => c).First();
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
