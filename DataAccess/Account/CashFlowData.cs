using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class CashFlowData : BaseData<CashFlow>
    {
        public override string TableName => "CashFlow";

        private const string LIST_BY_USER = @"SELECT c.* FROM CashFlow c WHERE c.UserId = @UserId";

        public List<CashFlow> ListByUserId(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<CashFlow>(LIST_BY_USER, parameters).ToList();
        }
    }
}
