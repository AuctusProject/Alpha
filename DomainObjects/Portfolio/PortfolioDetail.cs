using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Portfolio
{
    public class PortfolioDetail
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int PortfolioId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime Date { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Name { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Description { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public decimal? Price { get; set; }
        [DapperType(System.Data.DbType.Boolean)]
        public bool Enabled { get; set; }
    }
}
