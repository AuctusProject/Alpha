using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class Advisor
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public AdvisorType Type { get; set; }

        public AdvisorDetail Detail { get; set; }
    }
}
