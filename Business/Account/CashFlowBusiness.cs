using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Account
{
    public class CashFlowBusiness : BaseBusiness<CashFlow, CashFlowData>
    {
        public static decimal InitialDeposit = 10000;
        public CashFlowBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public CashFlow SetNew(int userId, decimal value)
        {
            var deposit = new CashFlow();
            deposit.UserId = userId;
            deposit.Value = value;
            return deposit;
        }

        public decimal GetUserBalance(int userId) {
            return Data.ListByUserId(userId).Sum(d => d.Value);
        }
    }
}
