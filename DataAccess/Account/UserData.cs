﻿using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class UserData : BaseData<User>
    {
        public override string TableName => "[User]";

        private const string SELECT_BY_API_KEY = @"SELECT u.* FROM [User] u INNER JOIN ApiAccess a ON a.UserId = u.Id 
                                                   WHERE a.CreationDate = (SELECT max(a2.CreationDate) FROM ApiAccess a2 WHERE a2.UserId = u.Id)
                                                   AND a.ApiKey = @ApiKey";

        public User Get(string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Email", email.ToLower().Trim(), DbType.AnsiString);
            return SelectByParameters<User>(parameters).SingleOrDefault();
        }

        public User GetByConfirmationCode(string confirmationCode)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ConfirmationCode", confirmationCode, DbType.AnsiString);
            return SelectByParameters<User>(parameters).SingleOrDefault();
        }

        public User Get(Guid guid)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ApiKey", guid.ToString(), DbType.AnsiString);
            return Query<User>(SELECT_BY_API_KEY, parameters).SingleOrDefault();
        }
    }
}
