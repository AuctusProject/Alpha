using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advice;
using Auctus.Model;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Service
{
    public class AdviceServices : BaseServices
    {
        public AdviceServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Advisor CreateAdvisor(string email, string name, string description, int period, double price)
        {
            return AdvisorBusiness.Create(email, name, description, period, price);
        }

        public AdvisorDetail UpdateAdvisor(string email, int advisorId, string description, int period, double price, bool enabled)
        {
            return AdvisorDetailBusiness.Create(email, advisorId, description, period, price, enabled);
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

        public Buy Buy(string email, int advisorId)
        {
            return BuyBusiness.Create(email, advisorId);
        }

        public Projections GetProjections(string email)
        {
            return ProjectionBusiness.GetProjections(email);
        }
    }
}
