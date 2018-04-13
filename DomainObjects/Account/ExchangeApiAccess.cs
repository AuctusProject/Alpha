using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class ExchangeApiAccess
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int ExchangeId { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string ApiKey { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string ApiSecretKey { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
    }
}
