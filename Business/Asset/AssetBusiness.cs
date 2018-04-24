using Auctus.DataAccess.Asset;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Asset
{
    public class AssetBusiness : BaseBusiness<Auctus.DomainObjects.Asset.Asset, AssetData>
    {
        public AssetBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public List<Auctus.DomainObjects.Asset.Asset> ListAssets()
        {
            string cacheKey = "Assets";
            var assets = MemoryCache.Get<List<Auctus.DomainObjects.Asset.Asset>>(cacheKey);
            if (assets == null)
            {
                assets = Data.SelectAll().ToList();
                if (assets != null)
                    MemoryCache.Set<List<Auctus.DomainObjects.Asset.Asset>>(cacheKey, assets, 1440);
            }
            return assets;
        }

        public List<Auctus.DomainObjects.Asset.Asset> ListAssetsIncludingUsd()
        {
            var assets = ListAssets();
            assets.Add(new DomainObjects.Asset.Asset()
            {
                Code = "USD",
                Id = 0,
                Name = "Dolar",
                Type = DomainObjects.Asset.AssetType.Traditional.Value
            });
            return assets;
        }

        public void UpdateAllAssetsValues()
        {
            var assets = ListAssets();

            foreach (var asset in assets)
            {
                AssetValueBusiness.UpdateAssetValue(asset);
            }
        }
    }
}
