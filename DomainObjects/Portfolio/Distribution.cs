using Auctus.Util.DapperAttributes;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Portfolio
{
    public class Distribution : MongoDomainObject
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int AssetId { get; set; }
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int ProjectionId { get; set; }
        public int PortfolioId { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double Percent { get; set; }

        [BsonIgnore]
        public Auctus.DomainObjects.Asset.Asset Asset { get; set; }
        [BsonIgnore]
        public Projection Projection { get; set; }
    }
}
