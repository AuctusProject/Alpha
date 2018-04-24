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
    public class AssetCurrentValueBusiness : BaseBusiness<AssetCurrentValue, AssetCurrentValueData>
    {
        public AssetCurrentValueBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public void UpdateAssetsCurrentValues()
        {
            var assets = AssetBusiness.ListAssets();
            var currentValuesDictionary = ExchangeApi.GetCurrentCryptoValues(assets.Select(a => a.Code));
            foreach(var currentValue in currentValuesDictionary)
            {
                var asset = assets.FirstOrDefault(a => a.Code == currentValue.Key);
                if (asset != null)
                {
                    var assetCurrentValue = new AssetCurrentValue()
                    {
                        AssetId = asset.Id,
                        Date = DateTime.UtcNow,
                        Value = currentValue.Value
                    };
                    Data.Update(assetCurrentValue);
                }
            }
        }
    }
}
