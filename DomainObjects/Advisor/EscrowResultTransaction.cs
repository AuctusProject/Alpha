using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class EscrowResultTransaction
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int EscrowResultId { get; set; }
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int TransactionId { get; set; }
    }
}
