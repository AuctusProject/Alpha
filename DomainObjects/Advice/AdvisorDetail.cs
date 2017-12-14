using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advice
{
    public class AdvisorDetail
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int AdvisorId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime Date { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Description { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double Price { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int Period { get; set; }
    }
}
