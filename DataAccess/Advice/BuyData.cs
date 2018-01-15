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

        private const string SELECT_PURCHASE = @"SELECT b.*, g.* FROM 
                                                 Buy b 
                                                 INNER JOIN Goal g ON g.Id = b.GoalId 
                                                 WHERE g.UserId = @UserId AND b.ExpirationDate > @Date";

        private const string SELECT_PURCHASE_WITH_PORTFOLIO = @"SELECT b.*, g.*, e.*, p.* FROM 
                                                                Buy b 
                                                                INNER JOIN Goal g ON g.Id = b.GoalId 
                                                                INNER JOIN Projection e ON e.Id = b.ProjectionId 
                                                                INNER JOIN Portfolio p ON p.Id = e.PortfolioId 
                                                                WHERE g.UserId = @UserId AND b.ExpirationDate > @Date";

        private const string SELECT_PURCHASE_COMPLETE = @"SELECT b.*, g.*, a.*, d.*, e.*, p.* FROM 
                                                          Buy b 
                                                          INNER JOIN Goal g ON g.Id = b.GoalId 
                                                          INNER JOIN Advisor a ON a.Id = b.AdvisorId 
                                                          INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id 
                                                          INNER JOIN Projection e ON e.Id = b.ProjectionId 
                                                          INNER JOIN Portfolio p ON p.Id = e.PortfolioId 
                                                          WHERE g.UserId = @UserId AND b.ExpirationDate > @Date AND
                                                          d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id AND d2.Date <= @Date)";

        public List<Buy> ListPurchases(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, Goal, Buy>(SELECT_PURCHASE,
                            (buy, goal) =>
                            {
                                buy.Goal = goal;
                                return buy;
                            }, "Id", parameters).ToList();
        }

        public List<Buy> ListPurchasesWithPortfolio(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, Goal, Projection, Portfolio, Buy>(SELECT_PURCHASE_WITH_PORTFOLIO,
                            (buy, goal, projection, portfolio) =>
                            {
                                buy.Goal = goal;
                                buy.Projection = projection;
                                buy.Projection.Portfolio = portfolio;
                                return buy;
                            }, "Id,Id,Id", parameters).ToList();
        }

        public List<Buy> ListPurchasesComplete(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, Goal, Advisor, AdvisorDetail, Projection, Portfolio, Buy>(SELECT_PURCHASE_COMPLETE,
                            (buy, goal, advisor, detail, projection, portfolio) =>
                            {
                                buy.Goal = goal;
                                buy.Advisor = advisor;
                                buy.Advisor.Detail = detail;
                                buy.Projection = projection;
                                buy.Projection.Portfolio = portfolio;
                                return buy;
                            }, "Id,Id,Id,Id,Id", parameters).ToList();
        }
    }
}
