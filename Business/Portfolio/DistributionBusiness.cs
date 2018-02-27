using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<Model.Portfolio.Distribution> ListByUserPortfolio(string email, int portfolioId)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchase = Task.Factory.StartNew(() => BuyBusiness.Get(user.Id, portfolioId));
            var portfolio = Task.Factory.StartNew(() => PortfolioBusiness.GetWithDetails(portfolioId));
            Task.WaitAll(purchase, portfolio);

            var purchased = purchase.Result != null && BuyBusiness.IsValidPurchase(purchase.Result) && purchase.Result.LastTransaction.TransactionStatus == TransactionStatus.Success;
            var owned = portfolio.Result != null && portfolio.Result.Advisor.UserId == user.Id;
            if (!purchased && !owned)
                throw new ArgumentException("Invalid portfolio distribution.");

            return List(new int[] { portfolio.Result.ProjectionId.Value }).Select(c => new Model.Portfolio.Distribution()
            {
                Id = c.AssetId,
                Code = c.Asset.Code,
                Name = c.Asset.Name,
                Percentage = c.Percent,
                Type = (int)c.Asset.Type
            }).OrderByDescending(c => c.Percentage).ToList();
        }
    }
}
