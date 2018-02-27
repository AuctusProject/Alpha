using Auctus.DomainObjects.Account;
using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class EscrowResult
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int BuyId { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public decimal BuyerTokenResult { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public decimal OwnerTokenResult { get; set; }

        public Transaction LastTransaction { get; set; }
    }
}
