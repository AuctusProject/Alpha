using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Asset;
using Dapper;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Asset
{
    public class AssetValueData : BaseMongo<AssetValue>
    {
        public override string CollectionName => "AssetValue";

        public AssetValue GetLastValue(int assetId)
        {
            var value = Collection.Find( x => x.AssetId == assetId).SortByDescending(x => x.Date).FirstOrDefault();

            return value;
        }

        public IEnumerable<AssetValue> List(IEnumerable<int> assetsIds, DateTime? startDate = null)
        {
            var filterBuilder = Builders<AssetValue>.Filter;
            var filter = filterBuilder.In(x => x.AssetId, assetsIds.ToArray());
            if (startDate.HasValue)
                filter = filter & filterBuilder.Gte(x => x.Date, startDate.Value);

            return Collection.Find(filter).ToList();
        }
    }
}
