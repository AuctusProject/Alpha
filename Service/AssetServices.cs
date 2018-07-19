using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Asset;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class AssetServices : BaseServices
    {
        public AssetServices(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public void UpdateAllAssetsValues()
        {
            AssetBusiness.UpdateAllAssetsValues();
        }

        public void CreateAssets()
        {
            AssetBusiness.CreateCoinMarketCapNotIncludedAssets();
        }

        public void UpdateAllAssetsCurrentValues()
        {
            AssetCurrentValueBusiness.UpdateAssetsCurrentValues();
        }

        public List<Asset> ListAssets()
        {
            return AssetBusiness.ListAssets();
        }
    }
}
