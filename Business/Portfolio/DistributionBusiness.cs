using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Portfolio
{
    public class DistributionBusiness : BaseBusiness<Distribution, DistributionData>
    {
        public DistributionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public List<Distribution> Create(string email, int portfolioId, Dictionary<int, double> distribution)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = PortfolioBusiness.GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            var projection = ProjectionBusiness.Get(portfolio.ProjectionId.Value);
            if (projection == null)
                throw new ArgumentException("Invalid projection.");

            var risk = PortfolioBusiness.GetRisk(projection.ProjectionValue, distribution);

            return ProjectionBusiness.Create(portfolio, projection.ProjectionValue, risk, projection.OptimisticProjectionValue,
                projection.PessimisticProjectionValue, distribution).Distribution;
        }

        public List<Distribution> SetNew(int projectionId, Dictionary<int, double> distribution)
        {
            if (distribution.Any(c => c.Value < 0 || c.Value > 100))
                throw new ArgumentException("Invalid asset distribution value.");
            if (distribution.Sum(c => c.Value) != 100)
                throw new ArgumentException("Asset distribution must match 100%.");

            var distributions = new List<Distribution>();
            foreach(KeyValuePair<int, double> key in distribution)
            {
                if (key.Value > 0)
                    distributions.Add(new Distribution()
                    {
                        AssetId = key.Key,
                        Percent = key.Value,
                        ProjectionId = projectionId
                    });
            }
            return distributions;
        }

        public List<Distribution> List(IEnumerable<int> projectionIds)
        {
            if (projectionIds.Count() == 0)
                return new List<Distribution>();

            var distributions = Data.List(projectionIds);
            var assets = AssetBusiness.ListAssets();
            distributions.ForEach(c => c.Asset = assets.Single(a => a.Id == c.AssetId));
            return distributions;
        }

        //public List<Model.PortfolioDistribution> ListPortfolioDistribution(string email)
        //{
        //    var user = UserBusiness.GetValidUser(email);
        //    var purchases = BuyBusiness.ListPurchases(user.Id);
        //    var distributions = List(purchases.Select(c => c.ProjectionId));
        //    List<Model.PortfolioDistribution> result = new List<Model.PortfolioDistribution>();
        //    foreach (Buy buy in purchases)
        //    {
        //        Model.PortfolioDistribution portfolioDistribution = new Model.PortfolioDistribution();
        //        portfolioDistribution.AdvisorId = buy.AdvisorId;
        //        portfolioDistribution.Distribution = distributions.Where(c => c.ProjectionId == buy.ProjectionId).Select(c =>
        //        new Model.PortfolioDistribution.Asset()
        //        {
        //            Code = c.Asset.Code,
        //            Id = c.Asset.Id,
        //            Name = c.Asset.Name,
        //            Type = (int)c.Asset.Type,
        //            Percentage = c.Percent 
        //        }
        //        ).ToList();
        //        result.Add(portfolioDistribution);
        //    }
        //    return result;
        //}
    }
}
