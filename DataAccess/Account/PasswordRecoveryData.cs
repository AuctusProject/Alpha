using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class PasswordRecoveryData : BaseSQL<PasswordRecovery>
    {
        public override string TableName => "PasswordRecovery";

        public PasswordRecovery Get(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<PasswordRecovery>(parameters).SingleOrDefault();
        }

        public PasswordRecovery Get(string token)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Token", token, DbType.AnsiString);
            return SelectByParameters<PasswordRecovery>(parameters).SingleOrDefault();
        }
    }
}
