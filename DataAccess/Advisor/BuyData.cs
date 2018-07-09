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
    public class BuyData : BaseSQL<Buy>
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

        private const string SELECT_PORTFOLIO_PURCHASE_AMOUNT = @"SELECT SUM(m.Price) Amount FROM 
                                                                 Buy m
                                                                 WHERE m.ExpirationDate IS NOT NULL AND m.PortfolioId = @PortfolioId ";

        private const string SELECT_PURCHASE_COMPLETE = @"SELECT b.*, t.*, p.*, d.*, g.* FROM
                                                        Buy b
                                                        INNER JOIN BuyTransaction bt ON bt.BuyId = b.Id
                                                        INNER JOIN [Transaction] t ON t.Id = bt.TransactionId
                                                        INNER JOIN Projection p ON p.Id = b.ProjectionId 
                                                        INNER JOIN PortfolioDetail d ON d.Id = b.PortfolioDetailId 
                                                        LEFT JOIN Goal g ON g.Id = b.GoalId 
                                                        WHERE b.UserId = @UserId AND b.PortfolioId = @PortfolioId AND
                                                        b.CreationDate = (SELECT max(b2.CreationDate) FROM Buy b2 WHERE b2.UserId = b.UserId AND b2.PortfolioId = b.PortfolioId)
                                                        AND t.CreationDate = (SELECT max(t2.CreationDate) FROM BuyTransaction bt2
                                                                            INNER JOIN [Transaction] t2 ON t2.Id = bt2.TransactionId
                                                                            WHERE bt2.BuyId = b.Id)";

        private const string SELECT_PENDING_ESCROWRESULT = @"SELECT b.*, p.* FROM 
                                                            Buy b 
                                                            INNER JOIN Portfolio p ON p.Id = b.PortfolioId
                                                            WHERE b.ExpirationDate IS NOT NULL AND b.ExpirationDate < @ExpirationDate
                                                            AND NOT EXISTS (SELECT 1 FROM EscrowResult e WHERE e.BuyId = b.Id)";

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

        public Buy Get(int userId, int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return Query<Buy, Transaction, Projection, PortfolioDetail, Goal, Buy>(SELECT_PURCHASE_COMPLETE,
                            (buy, trans, proj, detail, goal) =>
                            {
                                buy.LastTransaction = trans;
                                buy.Projection = proj;
                                buy.PortfolioDetail = detail;
                                buy.Goal = goal;
                                return buy;
                            }, "Id,Id,Id,Id", parameters).SingleOrDefault();
        }

        public List<Buy> ListPendingEscrowResult()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("ExpirationDate", DateTime.UtcNow, DbType.DateTime);
            return Query<Buy, DomainObjects.Portfolio.Portfolio, Buy>(SELECT_PENDING_ESCROWRESULT,
                            (buy, port) =>
                            {
                                buy.Portfolio = port;
                                return buy;
                            }, "Id", parameters).ToList();
        }

        public Buy GetSimple(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            return SelectByParameters<Buy>(parameters).SingleOrDefault();
        }

        public decimal? ListPortfolioPurchaseAmount(int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return Query<decimal?>(SELECT_PORTFOLIO_PURCHASE_AMOUNT, parameters).SingleOrDefault();
        }

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
