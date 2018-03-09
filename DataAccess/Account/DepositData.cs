using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class DepositData : BaseData<Deposit>
    {
        public override string TableName => "Deposit";

        private const string LIST_BY_USER = @"SELECT d.* FROM Deposit d WHERE d.UserId = @UserId";

        public List<Deposit> ListByUserId(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<Deposit>(LIST_BY_USER, parameters).ToList();
        }
    }
}
