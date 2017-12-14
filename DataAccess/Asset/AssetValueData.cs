using Auctus.DomainObjects.Asset;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Asset
{
    public class AssetValueData : BaseData<AssetValue>
    {
        public override string TableName => "AssetValue";

        public AssetValue GetLastValue(int assetId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AssetId", assetId, DbType.Int32);
            return SelectByParameters<AssetValue>(parameters, "Date desc").FirstOrDefault();   
        }
    }
}
