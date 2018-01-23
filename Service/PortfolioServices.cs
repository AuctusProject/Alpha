using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class PortfolioServices : BaseServices
    {
        public PortfolioServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public void UpdatePortfoliosHistory()
        {
            PortfolioBusiness.UpdateAllPortfoliosHistory();
        }

        public Portfolio CreatePortfolio(string email, int advisorId, int risk, double projection, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            return PortfolioBusiness.Create(email, advisorId, risk, projection, optimisticProjection, pessimisticProjection, distribution);
        }

        public List<Portfolio> ListPortfolio(string email)
        {
            return PortfolioBusiness.List(email);
        }

        public void DisablePortfolio(string email, int portfolioId)
        {
            PortfolioBusiness.Disable(email, portfolioId);
        }

        public Projection CreateProjection(string email, int portfolioId, double projection, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            return ProjectionBusiness.Create(email, portfolioId, projection, optimisticProjection, pessimisticProjection, distribution);
        }

        public List<Distribution> CreateDistribution(string email, int portfolioId, Dictionary<int, double> distribution)
        {
            return DistributionBusiness.Create(email, portfolioId, distribution);
        }

        public Model.Projections GetProjections(string email)
        {
            return ProjectionBusiness.GetProjections(email);
        }

        public List<Model.PortfolioHistory> ListHistory(string email)
        {
            return PortfolioHistoryBusiness.ListHistory(email);
        }

        public List<Model.PortfolioDistribution> ListPortfolioDistribution(string email)
        {
            return DistributionBusiness.ListPortfolioDistribution(email);
        }
    }
}
