using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class ApiAccessData : BaseData<ApiAccess>
    {
        public override string TableName => "ApiAccess";

        public List<ApiAccess> List(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<ApiAccess>(parameters).ToList();
        }
    }
}
