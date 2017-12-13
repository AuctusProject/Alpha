using Auctus.DomainObjects.Asset;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Asset
{
    public class AssetValueData : BaseData<AssetValue>
    {
        public override string TableName => "AssetValue";
    }
}
