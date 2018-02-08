using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class Wallet
    {
        [DapperKey]
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Address { get; set; }
    }
}
