using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advice
{
    public class Distribution
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int AssetId { get; set; }
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int ProjectionId { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double Percent { get; set; }
        
        public Auctus.DomainObjects.Asset.Asset Asset { get; set; }
    }
}
