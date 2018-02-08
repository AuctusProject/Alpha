using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class BuyTransaction
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int BuyId { get; set; }
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int TransactionId { get; set; }
    }
}
