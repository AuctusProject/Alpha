﻿using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class Buy
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int Days { get; set; }
        [DapperType(System.Data.DbType.Decimal)]
        public decimal Price { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime? ExpirationDate { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int PortfolioDetailId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int PortfolioId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int ProjectionId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int? GoalId { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }

        public Goal Goal { get; set; }
        public PortfolioDetail PortfolioDetail { get; set; }
        public Projection Projection { get; set; }
        public DomainObjects.Portfolio.Portfolio Portfolio { get; set; }
        public Transaction LastTransaction { get; set; }
    }
}
