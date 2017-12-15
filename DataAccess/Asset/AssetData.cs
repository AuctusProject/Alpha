using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Asset
{
    public class AssetData : BaseData<Auctus.DomainObjects.Asset.Asset>
    {
        public override string TableName => "Asset";

        public DomainObjects.Asset.Asset GetByCode(string code)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Code", code, DbType.String);
            return SelectByParameters<DomainObjects.Asset.Asset>(parameters).SingleOrDefault();
        }

        public void UpdateAssetValue(string code, DateTime start)
        {
            var dateAndValue = AlphaVantageApi.GetCloseAdjustedValues(code);
        }
    }
}
