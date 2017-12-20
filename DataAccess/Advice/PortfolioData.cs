using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class PortfolioData : BaseData<Portfolio>
    {
        public override string TableName => "Portfolio";

        private const string SELECT_VALID_BY_ADVISOR_AND_RISK = @"SELECT p.* FROM Portfolio p  
                                                                  INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                                  WHERE p.Risk = @Risk AND a.Id = @AdvisorId AND p.Disabled IS NULL";

        private const string SELECT_VALID_BY_OWNER = @"SELECT p.* FROM Portfolio p  
                                                       INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                       INNER JOIN [User] u ON u.Id = a.UserId  
                                                       WHERE p.Id = @Id AND u.Email = @Email AND p.Disabled IS NULL";

        private const string SELECT_LIST_BY_OWNER = @"SELECT p.*, j.* FROM 
                                                      Portfolio p  
                                                      INNER JOIN Projection j ON p.ProjectionId = j.Id
                                                      INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                      INNER JOIN [User] u ON u.Id = a.UserId  
                                                      WHERE u.Email = @Email AND p.Disabled IS NULL";

        private const string LIST_ALL =
            @"SELECT port.*, proj.*, dist.* FROM Portfolio port 
            INNER JOIN Projection proj on port.ProjectionId = proj.Id
            INNER JOIN Distribution dist on proj.Id = dist.ProjectionId";

        public Portfolio GetValidByAdvisorAndRisk(int advisorId, int risk)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AdvisorId", advisorId, DbType.Int32);
            parameters.Add("Risk", risk, DbType.Int32);
            return Query<Portfolio>(SELECT_VALID_BY_ADVISOR_AND_RISK, parameters).SingleOrDefault();
        }

        public Portfolio GetValidByOwner(string email, int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", portfolioId, DbType.Int32);
            parameters.Add("Email", email, DbType.AnsiString);
            return Query<Portfolio>(SELECT_VALID_BY_OWNER, parameters).SingleOrDefault();
        }

        public List<Portfolio> List(string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Email", email, DbType.AnsiString);
            return Query<Portfolio, Projection, Portfolio>(SELECT_LIST_BY_OWNER,
                            (port, proj) =>
                            {
                                port.Projection = proj;
                                return port;
                            }, "Id", parameters).ToList();
        }
        
        public IEnumerable<Portfolio> ListAll()
        {
            var cache = new Dictionary<int, Portfolio>();
            var cacheProjection = new Dictionary<int, Projection>();
            return Query<Portfolio, Projection, Distribution, Portfolio>(LIST_ALL,
                    (portfolio, projection, distribution) =>
                    {
                        if (!cache.ContainsKey(portfolio.Id))
                            cache.Add(portfolio.Id, portfolio);

                        var cached = cache[portfolio.Id];

                        if (!cacheProjection.ContainsKey(projection.Id))
                            cacheProjection.Add(projection.Id, projection);

                        if (cached.Projections == null)
                        {
                            cached.Projections = new List<Projection>();
                        }

                        var cachedProjection = cacheProjection[projection.Id];

                        if (!cached.Projections.Contains(cachedProjection))
                        {
                            cached.Projections.Add(cachedProjection);
                        }

                        if (cachedProjection.Distribution == null)
                        {
                            cachedProjection.Distribution = new List<Distribution>();
                        }
                        if (distribution != null)
                        {
                            cachedProjection.Distribution.Add(distribution);
                        }

                        return cached;
                    }, "Id, AssetId").Distinct();
        }

    }
}
