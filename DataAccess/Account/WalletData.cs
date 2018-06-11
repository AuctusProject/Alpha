using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class WalletData : BaseSQL<Wallet>
    {
        public override string TableName => "Wallet";

        public Wallet GetByUser(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<Wallet>(parameters).SingleOrDefault();
        }
    }
}
