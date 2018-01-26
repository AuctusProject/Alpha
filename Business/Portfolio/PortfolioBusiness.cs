using Auctus.DataAccess.Core;
using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Portfolio
{
    public class PortfolioBusiness : BaseBusiness<DomainObjects.Portfolio.Portfolio, PortfolioData>
    {
        public PortfolioBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public DomainObjects.Portfolio.Portfolio Create(string email, int advisorId, int risk, double projectionValue, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            if (risk < 1 || risk > 5)
                throw new ArgumentException("Inavild risk.");

            var advisor = AdvisorBusiness.GetWithOwner(advisorId, email);
            if (advisor == null)
                throw new ArgumentException("Invalid advisor.");

            var portfolio = GetValidByAdvisorAndRisk(advisorId, risk);
            if (portfolio != null)
                throw new ArgumentException("Already exist a portfolio registered for this risk.");

            var advisorDetail = AdvisorDetailBusiness.GetForAutoEnabled(advisorId);

            portfolio = new DomainObjects.Portfolio.Portfolio();
            portfolio.AdvisorId = advisorId;
            portfolio.CreationDate = DateTime.UtcNow;
            portfolio.Risk = risk;
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(portfolio);
                var projection = ProjectionBusiness.SetNew(portfolio.Id, projectionValue, optimisticProjection, pessimisticProjection);
                transaction.Insert(projection);
                var distributions = DistributionBusiness.SetNew(projection.Id, distribution);
                foreach (Distribution dist in distributions)
                    transaction.Insert(dist);

                portfolio.ProjectionId = projection.Id;
                transaction.Update(portfolio);
                if (advisorDetail != null)
                {
                    advisorDetail = AdvisorDetailBusiness.SetNew(advisorId, advisorDetail.Description, advisorDetail.Period, advisorDetail.Price, true);
                    transaction.Insert(advisorDetail);
                }
                projection.Distribution = distributions;
                portfolio.Projection = projection;
                transaction.Commit();
            }
            return portfolio;
        }

        internal Projection GetProjectionAtDate(DateTime date, DomainObjects.Portfolio.Portfolio portfolio)
        {
            return portfolio.Projections.Where(p => p.Date < date).OrderByDescending(p => p.Date).FirstOrDefault();
        }

        public void Disable(string email, int portfolioId)
        {
            var portfolio = GetValidByOwner(email, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            if (!List(portfolio.AdvisorId).Any(c => c.Id != portfolio.Id))
                throw new ArgumentException("Unique advisor's portfolio cannot be disabled.");

            portfolio.Disabled = DateTime.UtcNow;
            Data.Update(portfolio);
        }

        public DomainObjects.Portfolio.Portfolio GetByRisk(int advisorId, RiskType riskType)
        {
            string defaultPortfoliosKey = "DefaultPortfolios";
            List<DomainObjects.Portfolio.Portfolio> portfolios = null;
            if (advisorId == AdvisorBusiness.DefaultAdvisorId)
                portfolios = MemoryCache.Get<List<DomainObjects.Portfolio.Portfolio>>(defaultPortfoliosKey);
            if (portfolios == null)
            {
                portfolios = List(advisorId);
                if (advisorId == AdvisorBusiness.DefaultAdvisorId)
                    MemoryCache.Set<List<DomainObjects.Portfolio.Portfolio>>(defaultPortfoliosKey, portfolios);
            }

            if (portfolios.Count == 1)
                return portfolios.First();

            var sameRisk = portfolios.SingleOrDefault(c => c.RiskType == riskType);
            if (sameRisk != null)
                return sameRisk;

            var littleLower = portfolios.SingleOrDefault(c => c.RiskType.Value == (riskType.Value - 1));
            if (littleLower != null)
                return littleLower;

            var littleHigher = portfolios.SingleOrDefault(c => c.RiskType.Value == (riskType.Value + 1));
            if (littleHigher != null)
                return littleHigher;

            var lower = portfolios.SingleOrDefault(c => c.RiskType.Value == (riskType.Value - 2));
            if (lower != null)
                return lower;

            return portfolios.Single(c => c.RiskType.Value == (riskType.Value + 2));
        }

        public DomainObjects.Portfolio.Portfolio GetValidByAdvisorAndRisk(int advisorId, int risk)
        {
            return Data.GetValidByAdvisorAndRisk(advisorId, risk);
        }

        public DomainObjects.Portfolio.Portfolio GetValidByOwner(string email, int portfolioId)
        {
            return Data.GetValidByOwner(email, portfolioId);
        }

        public List<DomainObjects.Portfolio.Portfolio> List(string email)
        {
            var portfolio = Data.List(email);
            return FillPortfoliosDistribution(portfolio);
        }

        public List<DomainObjects.Portfolio.Portfolio> List(int advisorId)
        {
            return List(new int[] { advisorId }).Single().Value;
        }

        public List<DomainObjects.Portfolio.Portfolio> ListWithHistory(int advisorId)
        {
            var portfolios = List(advisorId);
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));
            
            Task.WaitAll(histories.ToArray());

            portfolios.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());
            return portfolios;
        }

        public Dictionary<int, List<DomainObjects.Portfolio.Portfolio>> List(IEnumerable<int> advisorId)
        {
            var portfolio = Data.ListByAdvisor(advisorId);
            return portfolio.GroupBy(c => c.AdvisorId, c => c, (k, v) => new KeyValuePair<int, List<DomainObjects.Portfolio.Portfolio>>(k, v.ToList())).ToDictionary(c => c.Key, c => c.Value);
        }

        private List<DomainObjects.Portfolio.Portfolio> FillPortfoliosDistribution(List<DomainObjects.Portfolio.Portfolio> portfolio)
        {
            var distributions = DistributionBusiness.List(portfolio.Select(c => c.ProjectionId.Value));
            portfolio.ForEach(c => c.Projection.Distribution = distributions.Where(d => d.ProjectionId == c.ProjectionId.Value).ToList());
            return portfolio;
        }

        public void UpdateAllPortfoliosHistory()
        {
            var portfolios = Data.ListAll();
            foreach(var portfolio in portfolios)
            {
                PortfolioHistoryBusiness.UpdatePortfolioHistory(portfolio);
            }
        }
    }
}
