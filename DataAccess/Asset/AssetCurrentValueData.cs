using Auctus.DomainObjects.Asset;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Asset
{
    public class AssetCurrentValueData : BaseData<AssetCurrentValue>
    {
        public override string TableName => "AssetCurrentValue";
    }
}
