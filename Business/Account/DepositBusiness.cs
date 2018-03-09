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
    public class DepositBusiness : BaseBusiness<Deposit, DepositData>
    {
        public static decimal InitialDeposit = 10000;
        public DepositBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Deposit SetNew(int userId, decimal value)
        {
            var deposit = new Deposit();
            deposit.UserId = userId;
            deposit.Value = value;
            return deposit;
        }

        public decimal GetUserBalance(int userId) {
            return Data.ListByUserId(userId).Sum(d => d.Value);
        }
    }
}
