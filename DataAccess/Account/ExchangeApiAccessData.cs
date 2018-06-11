using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class ExchangeApiAccessData : BaseSQL<ExchangeApiAccess>
    {
        public override string TableName => "ExchangeApiAccess";

        public List<ExchangeApiAccess> List(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<ExchangeApiAccess>(parameters).ToList();
        }

        public ExchangeApiAccess Get(int userId, int exchangeId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("ExchangeId", exchangeId, DbType.Int32);
            return SelectByParameters<ExchangeApiAccess>(parameters).FirstOrDefault();
        }
    }
}
