using Auctus.DataAccess.Asset;
using Auctus.DomainObjects.Asset;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.Business.Asset
{
    public class AssetValueBusiness : BaseBusiness<AssetValue, AssetValueData>
    {
        public AssetValueBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        internal AssetValue LastAssetValue(int assetId)
        {
            return Data.GetLastValue(assetId);
        }

        internal void UpdateAssetValue(DomainObjects.Asset.Asset asset)
        {
            var lastUpdatedValue = AssetValueBusiness.LastAssetValue(asset.Id)?.Date ?? new DateTime(2015, 8, 8);
            if (lastUpdatedValue >= DateTime.Now.Date)
            {
                return;
            }
            Dictionary<DateTime, double> assetDateAndValues = GetAssetValuesByDate(asset);
            CreateAssetValueForPendingDates(asset, lastUpdatedValue, assetDateAndValues);
        }

        private void CreateAssetValueForPendingDates(DomainObjects.Asset.Asset asset, DateTime lastUpdatedValue, Dictionary<DateTime, double> assetDateAndValues)
        {
            var pendingUpdate = assetDateAndValues?.Where(d => d.Key > lastUpdatedValue).OrderBy(v => v.Key);
            if (pendingUpdate != null)
            {
                foreach (var pending in pendingUpdate)
                {
                    var assetValue = new DomainObjects.Asset.AssetValue() { AssetId = asset.Id, Date = pending.Key, Value = pending.Value };
                    Data.Insert(assetValue);
                }
            }
        }

        private static Dictionary<DateTime, double> GetAssetValuesByDate(DomainObjects.Asset.Asset asset)
        {
            Dictionary<DateTime, double> assetDateAndValues;
            if (asset.Type == DomainObjects.Asset.AssetType.Traditional)
            {
                assetDateAndValues = AlphaVantageApi.GetCloseAdjustedValues(asset.Code);
            }
            else if (asset.Type == DomainObjects.Asset.AssetType.Crypto)
            {
                assetDateAndValues = AlphaVantageApi.GetCloseCryptoValue(asset.Code);
            }
            else
                throw new InvalidOperationException();
            return assetDateAndValues;
        }
    }
}
