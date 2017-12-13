using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class GoalOptions
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public String Description { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int Risk { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int Position { get; set; }
    }
}
