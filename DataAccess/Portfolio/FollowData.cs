using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Portfolio
{
    public class FollowData : BaseData<Follow>
    {
        public override string TableName => "Buy";

        private const string SELECT_USER_FOLLOWS_BY_ADVISOR = @"SELECT b.*, p.* FROM 
                                                                Follow f 
                                                                INNER JOIN Portfolio p ON p.Id = f.PortfolioId 
                                                                WHERE f.UserId = @UserId AND p.AdvisorId = @AdvisorId ";

        private const string SELECT_ADVISOR_FOLLOW_QTY = @"SELECT m.AdvisorId, COUNT(f.Id) Qty FROM 
                                                             FOLLOW f
                                                             INNER JOIN Portfolio m ON m.Id = f.PortfolioId
                                                             WHERE ({0})
                                                             GROUP BY m.AdvisorId";

        private const string SELECT_PORTFOLIO_FOLLOWERS_QTY = @"SELECT m.PortfolioId, COUNT(m.Id) Qty FROM 
                                                             Follow m
                                                             WHERE ({0})
                                                             GROUP BY m.PortfolioId";

        private const string SELECT_FOLLOW_COMPLETE = @"SELECT f.*, p.*, d.* FROM
                                                        Follow f
                                                        INNER JOIN Projection p ON p.Id = f.ProjectionId 
                                                        INNER JOIN PortfolioDetail d ON d.Id = f.PortfolioDetailId 
                                                        WHERE f.UserId = @UserId AND f.PortfolioId = @PortfolioId ";

        public List<Follow> ListFollowing(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<Follow>(parameters).ToList();
        }

        public Follow Get(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            return SelectByParameters<Follow>(parameters).SingleOrDefault();
        }

        public List<Follow> ListUserAdvisorFollows(int userId, int advisorId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("AdvisorId", advisorId, DbType.Int32);
            return Query<Follow, DomainObjects.Portfolio.Portfolio, Follow>(SELECT_USER_FOLLOWS_BY_ADVISOR,
                            (follow, port) =>
                            {
                                follow.Portfolio = port;
                                return follow;
                            }, "Id,Id", parameters).ToList();
        }

        public Follow Get(int userId, int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return Query<Follow, Projection, PortfolioDetail, Follow>(SELECT_FOLLOW_COMPLETE,
                            (follow, proj, detail) =>
                            {
                                follow.Projection = proj;
                                follow.PortfolioDetail = detail;
                                return follow;
                            }, "Id,Id,Id,Id", parameters).SingleOrDefault();
        }

        public Dictionary<int, int> ListPortfoliosFollowers(IEnumerable<int> portfolioIds)
        {
            return ListPurchasesQty(portfolioIds, "PortfolioId", SELECT_PORTFOLIO_FOLLOWERS_QTY);
        }

        public Dictionary<int, int> ListAdvisorsFollowers(IEnumerable<int> advisorIds)
        {
            return ListPurchasesQty(advisorIds, "AdvisorId", SELECT_ADVISOR_FOLLOW_QTY);
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
