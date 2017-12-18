using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public class Goal
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? TargetAmount { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? StartingAmount { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public double? MonthlyContribution { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int? Timeframe { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int? Risk { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int GoalOptionId { get; set; }

        public GoalOption GoalOption { get; set; }
    }
}
