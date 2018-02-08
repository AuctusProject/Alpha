using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class Transaction
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string TransactionHash { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public TransactionStatus TransactionStatus { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime? ProcessedDate { get; set; }
    }
}
