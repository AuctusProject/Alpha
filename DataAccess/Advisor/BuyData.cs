using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class BuyData : BaseData<Buy>
    {
        public override string TableName => "Buy";

        private const string SELECT_USER_PURCHASE = @"SELECT b.*, t.* FROM 
                                                        Buy b 
                                                        INNER JOIN BuyTransaction bt ON bt.BuyId = b.Id
                                                        INNER JOIN [Transaction] t ON t.Id = bt.TransactionId
                                                        WHERE b.UserId = @UserId AND
                                                        t.CreationDate = (SELECT max(t2.CreationDate) FROM BuyTransaction bt2 
                                                                            INNER JOIN [Transaction] t2 ON t2.Id = bt2.TransactionId
                                                                            WHERE bt2.BuyId = b.Id)";

        private const string SELECT_USER_PURCHASE_BY_ADVISOR = @"SELECT b.*, t.*, p.* FROM 
                                                                Buy b 
                                                                INNER JOIN BuyTransaction bt ON bt.BuyId = b.Id
                                                                INNER JOIN [Transaction] t ON t.Id = bt.TransactionId
                                                                INNER JOIN Portfolio p ON p.Id = b.PortfolioId 
                                                                WHERE b.UserId = @UserId AND p.AdvisorId = @AdvisorId AND
                                                                t.CreationDate = (SELECT max(t2.CreationDate) FROM BuyTransaction bt2 
                                                                                    INNER JOIN [Transaction] t2 ON t2.Id = bt2.TransactionId
                                                                                    WHERE bt2.BuyId = b.Id)";

        private const string SELECT_PURCHASE = @"SELECT b.*, t.* FROM 
                                                Buy b 
                                                INNER JOIN BuyTransaction bt ON bt.BuyId = b.Id
                                                INNER JOIN [Transaction] t ON t.Id = bt.TransactionId
                                                WHERE b.Id = @Id AND
                                                t.CreationDate = (SELECT max(t2.CreationDate) FROM BuyTransaction bt2 
                                                                    INNER JOIN [Transaction] t2 ON t2.Id = bt2.TransactionId
                                                                    WHERE bt2.BuyId = b.Id)";

        private const string SELECT_ADVISOR_PURCHASE_QTY = @"SELECT m.AdvisorId, COUNT(b.Id) Qty FROM 
                                                             Buy b
                                                             INNER JOIN Portfolio m ON m.Id = b.PortfolioId
                                                             WHERE b.ExpirationDate IS NOT NULL AND ({0})
                                                             GROUP BY m.AdvisorId";

        private const string SELECT_PORTFOLIO_PURCHASE_QTY = @"SELECT m.PortfolioId, COUNT(m.Id) Qty FROM 
                                                             Buy m
                                                             WHERE m.ExpirationDate IS NOT NULL AND ({0})
                                                             GROUP BY m.PortfolioId";

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
            return Query<Buy, Transaction, Buy>(SELECT_USER_PURCHASE,
                            (buy, trans) =>
                            {
                                buy.LastTransaction = trans;
                                return buy;
                            }, "Id", parameters).ToList();
        }

        public Buy Get(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            return Query<Buy, Transaction, Buy>(SELECT_PURCHASE,
                            (buy, trans) =>
                            {
                                buy.LastTransaction = trans;
                                return buy;
                            }, "Id", parameters).SingleOrDefault();
        }

        public List<Buy> ListUserAdvisorPurchases(int userId, int advisorId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("AdvisorId", advisorId, DbType.Int32);
            return Query<Buy, Transaction, DomainObjects.Portfolio.Portfolio, Buy>(SELECT_USER_PURCHASE_BY_ADVISOR,
                            (buy, trans, port) =>
                            {
                                buy.LastTransaction = trans;
                                buy.Portfolio = port;
                                return buy;
                            }, "Id,Id", parameters).ToList();
        }

        //public List<Buy> ListPurchasesWithPortfolio(int userId)
        //{
        //    DynamicParameters parameters = new DynamicParameters();
        //    parameters.Add("UserId", userId, DbType.Int32);
        //    parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
        //    return Query<Buy, Goal, Projection, DomainObjects.Portfolio.Portfolio, Buy>(SELECT_PURCHASE_WITH_PORTFOLIO,
        //                    (buy, goal, projection, portfolio) =>
        //                    {
        //                        buy.Goal = goal;
        //                        buy.Projection = projection;
        //                        buy.Projection.Portfolio = portfolio;
        //                        return buy;
        //                    }, "Id,Id,Id", parameters).ToList();
        //}

        //public List<Buy> ListPurchasesComplete(int userId)
        //{
        //    DynamicParameters parameters = new DynamicParameters();
        //    parameters.Add("UserId", userId, DbType.Int32);
        //    parameters.Add("Date", DateTime.UtcNow, DbType.DateTime);
        //    return Query<Buy, Goal, DomainObjects.Advisor.Advisor, AdvisorDetail, Projection, DomainObjects.Portfolio.Portfolio, Buy>(SELECT_PURCHASE_COMPLETE,
        //                    (buy, goal, advisor, detail, projection, portfolio) =>
        //                    {
        //                        buy.Goal = goal;
        //                        buy.Advisor = advisor;
        //                        buy.Advisor.Detail = detail;
        //                        buy.Projection = projection;
        //                        buy.Projection.Portfolio = portfolio;
        //                        return buy;
        //                    }, "Id,Id,Id,Id,Id", parameters).ToList();
        //}
        
        public Dictionary<int, int> ListPortfoliosPurchases(IEnumerable<int> portfolioIds)
        {
            return ListPurchasesQty(portfolioIds, "PortfolioId", SELECT_PORTFOLIO_PURCHASE_QTY);
        }

        public Dictionary<int, int> ListAdvisorsPurchases(IEnumerable<int> advisorIds)
        {
            return ListPurchasesQty(advisorIds, "AdvisorId", SELECT_ADVISOR_PURCHASE_QTY);
        }

        private Dictionary<int, int> ListPurchasesQty(IEnumerable<int> ids, string columnName, string query)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            if (ids.Any())
            {
                List<string> restrictions = new List<string>();
                DynamicParameters parameters = new DynamicParameters();
                for (int i = 0; i < ids.Count(); ++i)
                {
                    var parameterName = string.Format("@id{0}", i);
                    restrictions.Add(string.Format("m.{0}={1}", columnName, parameterName));
                    parameters.Add(parameterName, ids.ElementAt(i), DbType.Int32);
                }
                var qty = Query(string.Format(query, string.Join(" OR ", restrictions)), parameters);

                foreach (IDictionary<string, object> pair in qty)
                    result.Add(Convert.ToInt32(pair[columnName]), Convert.ToInt32(pair["Qty"]));
            }
            return result;
        }
    }
}
