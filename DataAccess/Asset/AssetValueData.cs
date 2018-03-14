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

        private readonly string SQL_LIST_BY_ASSET_ID = @"SELECT TOP 1 av.* 
                                                FROM 
                                                AssetValue av
                                                where AssetId = @AssetId
                                                ORDER BY Date desc";

        private readonly string SQL_LIST_ASSET_FROM_DATE = @"SELECT av.* 
                                                FROM 
                                                AssetValue av
                                                where Date = @Date";

        private readonly string SQL_LIST_BY_ASSETS_IDS = @"SELECT av.* 
                                                FROM 
                                                AssetValue av
                                                where AssetId IN @AssetId
                                                AND Date >= @Date 
                                                ORDER BY Date desc";

        public AssetValue GetLastValue(int assetId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AssetId", assetId, DbType.Int32);
            return Query<AssetValue>(SQL_LIST_BY_ASSET_ID, parameters).FirstOrDefault();   
        }

        public IEnumerable<AssetValue> GetAssetValuesFromDate(DateTime date)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Date", date, DbType.DateTime);
            return Query<AssetValue>(SQL_LIST_ASSET_FROM_DATE, parameters);
        }

        public IEnumerable<AssetValue> List(IEnumerable<int> assetsIds, DateTime startDate)
        {
            var query = SQL_LIST_BY_ASSETS_IDS;
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AssetId", assetsIds, ((DbType)(-1)));
            parameters.Add("Date", startDate, DbType.DateTime);

            return Query<AssetValue>(query, parameters);
        }
    }
}
