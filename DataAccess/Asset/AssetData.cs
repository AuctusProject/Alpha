using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Asset
{
    public class AssetData : BaseData<Auctus.DomainObjects.Asset.Asset>
    {
        public override string TableName => "Asset";
    }
}
