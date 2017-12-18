using Auctus.DataAccess.Advice;
using Auctus.DataAccess.Core;
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

        public void Disable(string email, int portfolioId)
        {
            var portfolio = GetValidByOwner(email, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            portfolio.Disabled = DateTime.UtcNow;
            Data.Update(portfolio);
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
            var distributions = DistributionBusiness.List(portfolio.Select(c => c.ProjectionId.Value));
            portfolio.ForEach(c => c.Projection.Distribution = distributions.Where(d => d.ProjectionId == c.ProjectionId.Value).ToList());
            return portfolio;
        }
    }
}
