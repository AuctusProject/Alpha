using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Asset
{
    public class Asset
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Name { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Code { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int Type { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int CoinMarketCapId { get; set; }
    }
}
