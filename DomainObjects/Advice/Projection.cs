using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advice
{
    public class Projection
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int PortfolioId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime Date { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double ProjectionValue { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? OptimisticProjection { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? PessimisticProjection { get; set; }

        public Portfolio Portfolio { get; set; }
        public List<Distribution> Distribution { get; set; }
    }
}
