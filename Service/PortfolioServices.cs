using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class PortfolioServices : BaseServices
    {
        public PortfolioServices(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public void UpdatePortfoliosHistory()
        {
            PortfolioBusiness.UpdateAllPortfoliosHistory();
        }

        public Portfolio CreatePortfolio(string email, int advisorId, double price, string name, string description, double projection, 
            double? optimisticProjection, double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            return PortfolioBusiness.Create(email, advisorId, price, name, description, projection, optimisticProjection, pessimisticProjection, distribution);
        }

        public Portfolio UpdatePortfolio(string email, int portfolioId, double price, string name, string description)
        {
            return PortfolioBusiness.Update(email, portfolioId, price, name, description);
        }

        public List<Model.Portfolio> ListPortfolios(string email)
        {
            return PortfolioBusiness.List(email);
        }

        public void DisablePortfolio(string email, int portfolioId)
        {
            PortfolioBusiness.Disable(email, portfolioId);
        }

        public Projection CreateProjection(string email, int portfolioId, double projection, int risk, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            return ProjectionBusiness.Create(email, portfolioId, projection, RiskType.Get(risk), optimisticProjection, pessimisticProjection, distribution);
        }

        public List<Distribution> CreateDistribution(string email, int portfolioId, Dictionary<int, double> distribution)
        {
            return DistributionBusiness.Create(email, portfolioId, distribution);
        }
        
        //public Model.Projections GetProjections(string email)
        //{
        //    return ProjectionBusiness.GetProjections(email);
        //}

        //public List<Model.PortfolioHistory> ListHistory(string email)
        //{
        //    return PortfolioHistoryBusiness.ListHistory(email);
        //}

        //public List<Model.PortfolioDistribution> ListPortfolioDistribution(string email)
        //{
        //    return DistributionBusiness.ListPortfolioDistribution(email);
        //}
    }
}
