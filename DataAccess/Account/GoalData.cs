using Auctus.DomainObjects.Account;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class GoalData : BaseSQL<Goal>
    {
        public override string TableName => "Goal";

        private const string SELECT_CURRENT = @"SELECT g.* FROM Goal g WHERE g.UserId = @UserId AND g.CreationDate = (SELECT max(g2.CreationDate) FROM Goal g2 WHERE g2.UserId = g.UserId)";

        public Goal GetCurrent(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<Goal>(SELECT_CURRENT, parameters).SingleOrDefault();
        }
    }
}
