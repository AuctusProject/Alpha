﻿using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class GoalOption
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

        public RiskType RiskType { get { return RiskType.Get(Risk); } }
    }
}
