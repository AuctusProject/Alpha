using Auctus.DataAccess.Advice;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Advice
{
    public class PortfolioBusiness : BaseBusiness<Portfolio, PortfolioData>
    {
        public PortfolioBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Portfolio Create(string email, int advisorId, int risk, double projectionValue, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            if (risk < 0 || risk > 100)
                throw new ArgumentException("Inavild risk.");

            var advisor = AdvisorBusiness.GetWithOwner(advisorId, email);
            if (advisor == null)
                throw new ArgumentException("Invalid advisor.");

            var portfolio = GetValidByAdvisorAndRisk(advisorId, risk);
            if (portfolio != null)
                throw new ArgumentException("Already exist a portfolio registered for this risk.");

            var advisorDetail = AdvisorDetailBusiness.GetForAutoEnabled(advisorId);

            portfolio = new Portfolio();
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

        internal Projection GetProjectionAtDate(DateTime date, Portfolio portfolio)
        {
            return portfolio.Projections.Where(p => p.Date < date).OrderByDescending(p => p.Date).FirstOrDefault();
        }

        public void Disable(string email, int portfolioId)
        {
            var portfolio = GetValidByOwner(email, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            if (!Data.ListByAdvisor(portfolio.AdvisorId).Any(c => c.Id != portfolio.Id))
                throw new ArgumentException("Unique advisor's portfolio cannot be disabled.");

            portfolio.Disabled = DateTime.UtcNow;
            Data.Update(portfolio);
        }

        public Portfolio GetByRisk(int advisorId, RiskType riskType)
        {
            string defaultPortfoliosKey = "DefaultPortfolios";
            List<Portfolio> portfolios = null;
            if (advisorId == AdvisorBusiness.DefaultAdvisorId)
                portfolios = MemoryCache.Get<List<Portfolio>>(defaultPortfoliosKey);
            if (portfolios == null)
            {
                portfolios = Data.ListByAdvisor(advisorId);
                if (advisorId == AdvisorBusiness.DefaultAdvisorId)
                    MemoryCache.Set<List<Portfolio>>(defaultPortfoliosKey, portfolios);
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

        public Portfolio GetValidByAdvisorAndRisk(int advisorId, int risk)
        {
            return Data.GetValidByAdvisorAndRisk(advisorId, risk);
        }

        public Portfolio GetValidByOwner(string email, int portfolioId)
        {
            return Data.GetValidByOwner(email, portfolioId);
        }

        public List<Portfolio> List(string email)
        {
            var portfolio = Data.List(email);
            return FillPortfoliosDistribution(portfolio);
        }
        public List<Portfolio> List(int advisorId)
        {
            var portfolio = Data.ListByAdvisor(advisorId);
            return FillPortfoliosDistribution(portfolio);
        }

        private List<Portfolio> FillPortfoliosDistribution(List<Portfolio> portfolio)
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
