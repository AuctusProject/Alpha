using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class BuyData : BaseData<Buy>
    {
        public override string TableName => "Buy";

        private const string SELECT_BOUGHT = @"SELECT b.* FROM Buy b INNER JOIN Goal g ON g.Id = b.GoalId WHERE g.UserId = @UserId AND b.ExpirationDate > @Date";

        public List<Buy> ListBought(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy>(SELECT_BOUGHT, parameters).ToList();
        }

    }
}
