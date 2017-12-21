using Auctus.DomainObjects.Account;
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

        private const string SELECT_BOUGHT = @"SELECT b.*, g.* FROM 
                                               Buy b 
                                               INNER JOIN Goal g ON g.Id = b.GoalId 
                                               WHERE g.UserId = @UserId AND b.ExpirationDate > @Date";

        private const string SELECT_BOUGHT_WITH_ADVISOR = @"SELECT b.*, g., a.*, d.* FROM 
                                                            Buy b 
                                                            INNER JOIN Goal g ON g.Id = b.GoalId 
                                                            INNER JOIN Advisor a ON a.Id = b.AdvisorId 
                                                            INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id 
                                                            WHERE g.UserId = @UserId AND b.ExpirationDate > @Date AND
                                                            d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id AND d2.Date <= @Date)";

        public List<Buy> ListBought(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, Goal, Buy>(SELECT_BOUGHT,
                            (buy, goal) =>
                            {
                                buy.Goal = goal;
                                return buy;
                            }, "Id", parameters).ToList();
        }

        public List<Buy> ListBoughtWithAdvisor(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, Goal, Advisor, AdvisorDetail, Buy>(SELECT_BOUGHT_WITH_ADVISOR,
                            (buy, goal, advisor, detail) =>
                            {
                                buy.Goal = goal;
                                buy.Advisor = advisor;
                                buy.Advisor.Detail = detail;
                                return buy;
                            }, "Id,Id,Id", parameters).ToList();
        }
    }
}
