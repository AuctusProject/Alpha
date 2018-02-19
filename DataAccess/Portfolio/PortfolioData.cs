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
    public class PortfolioData : BaseData<DomainObjects.Portfolio.Portfolio>
    {
        public override string TableName => "Portfolio";

        private const string SELECT_VALID_BY_OWNER = @"SELECT p.*, d.* FROM Portfolio p  
                                                       INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                       INNER JOIN PortfolioDetail d on d.PortfolioId = p.Id 
                                                       WHERE p.Id = @Id AND a.UserId = @UserId AND d.Enabled = 1 AND
                                                       d.Date = (SELECT max(d2.Date) FROM PortfolioDetail d2 WHERE d2.PortfolioId = p.Id)";

        private const string SELECT_LIST_BY_OWNER = @"SELECT p.*, e.*, j.*, a.*, d.* FROM 
                                                      Portfolio p  
                                                      INNER JOIN Projection j ON p.ProjectionId = j.Id
                                                      INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                      INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                      INNER JOIN PortfolioDetail e on e.PortfolioId = p.Id 
                                                      WHERE 
                                                      a.UserId = @UserId AND 
                                                      d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) AND
                                                      e.Date = (SELECT max(e2.Date) FROM PortfolioDetail e2 WHERE e2.PortfolioId = p.Id)";

        private const string SELECT_LIST_BY_ADVISOR = @"SELECT p.*, d.*, j.* FROM 
                                                      Portfolio p  
                                                      INNER JOIN PortfolioDetail d on d.PortfolioId = p.Id
                                                      INNER JOIN Projection j ON p.ProjectionId = j.Id
                                                      WHERE {0}
                                                      d.Date = (SELECT max(d2.Date) FROM PortfolioDetail d2 WHERE d2.PortfolioId = p.Id) AND ({1})";

        private const string LIST_ALL =
            @"SELECT port.*, proj.*, dist.* FROM Portfolio port 
            INNER JOIN Projection proj on port.ProjectionId = proj.Id
            INNER JOIN Distribution dist on proj.Id = dist.ProjectionId";
        
        private const string LIST_ALL_VALIDS = @"SELECT p.*, e.*, j.*, a.*, d.* FROM 
                                                Portfolio p  
                                                INNER JOIN Projection j ON p.ProjectionId = j.Id
                                                INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                INNER JOIN PortfolioDetail e on e.PortfolioId = p.Id 
                                                WHERE 
                                                d.Enabled = 1 AND e.Enabled = 1 AND a.Type = 0 AND
                                                d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) AND
                                                e.Date = (SELECT max(e2.Date) FROM PortfolioDetail e2 WHERE e2.PortfolioId = p.Id)";

        private const string SELECT_COMPLETE = @"SELECT p.*, e.*, j.*, a.*, d.* FROM 
                                                Portfolio p  
                                                INNER JOIN Projection j ON p.ProjectionId = j.Id
                                                INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                INNER JOIN PortfolioDetail e on e.PortfolioId = p.Id 
                                                WHERE 
                                                p.Id = @Id AND
                                                d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) AND
                                                e.Date = (SELECT max(e2.Date) FROM PortfolioDetail e2 WHERE e2.PortfolioId = p.Id)";

        private const string SELECT_WITH_DETAIL = @"SELECT p.*, d.*, a.*, e.* FROM 
                                                    Portfolio p 
                                                    INNER JOIN PortfolioDetail d on d.PortfolioId = p.Id
                                                    INNER JOIN Advisor a ON a.Id = p.AdvisorId  
                                                    INNER JOIN AdvisorDetail e ON e.AdvisorId = a.Id
                                                    WHERE 
                                                    d.Date = (SELECT max(d2.Date) FROM PortfolioDetail d2 WHERE d2.PortfolioId = p.Id) AND
                                                    e.Date = (SELECT max(e2.Date) FROM AdvisorDetail e2 WHERE e2.AdvisorId = a.Id) AND {0}";
        
        public DomainObjects.Portfolio.Portfolio GetValidByOwner(int userId, int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", portfolioId, DbType.Int32);
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, DomainObjects.Portfolio.Portfolio>(SELECT_VALID_BY_OWNER,
                (port, detail) =>
                {
                    port.Detail = detail;
                    return port;
                }, "Id", parameters).SingleOrDefault();
        }
        
        public List<DomainObjects.Portfolio.Portfolio> ListByAdvisor(IEnumerable<int> advisorsId, bool onlyEnabled)
        {
            List<string> restrictions = new List<string>();
            DynamicParameters parameters = new DynamicParameters();
            for (int i = 0; i < advisorsId.Count(); ++i)
            {
                var parameterName = string.Format("@advisor{0}", i);
                restrictions.Add(string.Format("p.AdvisorId={0}", parameterName));
                parameters.Add(parameterName, advisorsId.ElementAt(i), DbType.Int32);
            }

            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, Projection, DomainObjects.Portfolio.Portfolio>(
                string.Format(SELECT_LIST_BY_ADVISOR, onlyEnabled ? " d.Enabled = 1 AND " : "", string.Join(" OR ", restrictions)),
                            (port, detail, proj) =>
                            {
                                port.Detail = detail;
                                port.Projection = proj;
                                return port;
                            }, "Id,Id", parameters).ToList();
        }

        public List<DomainObjects.Portfolio.Portfolio> ListOwn(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, Projection, DomainObjects.Advisor.Advisor, AdvisorDetail, DomainObjects.Portfolio.Portfolio>(SELECT_LIST_BY_OWNER,
                            (port, portdet, proj, adv, advdet) =>
                            {
                                port.Advisor = adv;
                                port.Advisor.Detail = advdet;
                                port.Projection = proj;
                                port.Detail = portdet;
                                return port;
                            }, "Id,Id,Id,Id", parameters).ToList();
        }

        public List<DomainObjects.Portfolio.Portfolio> ListAllValids()
        {
            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, Projection, DomainObjects.Advisor.Advisor, AdvisorDetail, DomainObjects.Portfolio.Portfolio>(LIST_ALL_VALIDS,
                            (port, portdet, proj, adv, advdet) =>
                            {
                                port.Advisor = adv;
                                port.Advisor.Detail = advdet;
                                port.Projection = proj;
                                port.Detail = portdet;
                                return port;
                            }, "Id,Id,Id,Id").ToList();
        }

        public DomainObjects.Portfolio.Portfolio Get(int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", portfolioId, DbType.Int32);
            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, Projection, DomainObjects.Advisor.Advisor, AdvisorDetail, 
                DomainObjects.Portfolio.Portfolio>(SELECT_COMPLETE,
                            (port, portdet, proj, adv, advdet) =>
                            {
                                port.Advisor = adv;
                                port.Advisor.Detail = advdet;
                                port.Projection = proj;
                                port.Detail = portdet;
                                return port;
                            }, "Id,Id,Id,Id", parameters).SingleOrDefault();
        }

        public IEnumerable<DomainObjects.Portfolio.Portfolio> ListAll()
        {
            var cache = new Dictionary<int, DomainObjects.Portfolio.Portfolio>();
            var cacheProjection = new Dictionary<int, Projection>();
            return Query<DomainObjects.Portfolio.Portfolio, Projection, Distribution, DomainObjects.Portfolio.Portfolio>(LIST_ALL,
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

        public List<DomainObjects.Portfolio.Portfolio> ListWithDetails(IEnumerable<int> portfolioIds)
        {
            List<string> restrictions = new List<string>();
            DynamicParameters parameters = new DynamicParameters();
            for (int i = 0; i < portfolioIds.Count(); ++i)
            {
                var parameterName = string.Format("@Id{0}", i);
                restrictions.Add(string.Format("p.Id={0}", parameterName));
                parameters.Add(parameterName, portfolioIds.ElementAt(i), DbType.Int32);
            }

            return Query<DomainObjects.Portfolio.Portfolio, PortfolioDetail, DomainObjects.Advisor.Advisor, AdvisorDetail, 
                DomainObjects.Portfolio.Portfolio>(string.Format(SELECT_WITH_DETAIL, string.Join(" OR ", restrictions)),
                            (p, pd, a, ad) =>
                            {
                                p.Detail = pd;
                                p.Advisor = a;
                                p.Advisor.Detail = ad;
                                return p;
                            }, "Id,Id,Id", parameters).ToList();
        }
    }
}
