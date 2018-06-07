using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Portfolio
{
    public class PortfolioHistory
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int PortfolioId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime Date { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double RealValue { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? ProjectionValue { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? PessimisticProjectionValue { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? OptimisticProjectionValue { get; set; }
    }
}
