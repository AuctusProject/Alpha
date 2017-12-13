using Auctus.DataAccess.Asset;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Asset
{
    public class AssetBusiness : BaseBusiness<Auctus.DomainObjects.Asset.Asset, AssetData>
    {
        public AssetBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }
    }
}
