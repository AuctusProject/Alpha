using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Asset
{
    public class AssetValue
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int AssetId { get; set; }
        [DapperKey]
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime Date { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double Value { get; set; }
    }
}
