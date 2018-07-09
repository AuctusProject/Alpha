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

        public List<Distribution> SetNew(int projectionId, int portfolioId, Dictionary<int, double> distribution, DateTime date)
        {
            var distributions = new List<Distribution>();
            foreach(KeyValuePair<int, double> key in distribution)
            {
                if (key.Value > 0)
                    distributions.Add(new Distribution()
                    {
                        AssetId = key.Key,
                        Percent = key.Value,
                        ProjectionId = projectionId,
                        PortfolioId = portfolioId,
                        Date = date
                    });
            }
            return distributions;
        }

        public List<Distribution> List(IEnumerable<int> projectionIds)
        {
            if (projectionIds.Count() == 0)
                return new List<Distribution>();

            var distributions = Data.List(projectionIds);
            FillDistributionsWithAssets(distributions);
            return distributions;
        }

        public List<Distribution> ListFromPortfolioId(IEnumerable<int> portfolioId)
        {
            var distributions = Data.ListFromPortfolio(portfolioId);
            FillDistributionsWithAssets(distributions);
            return distributions;
        }

        public List<Distribution> ListFromPortfolioId(int portfolioId)
        {
            var distributions = Data.ListFromPortfolioWithProjection(portfolioId);
            FillDistributionsWithAssets(distributions);
            return distributions;
        }

        private void FillDistributionsWithAssets(List<Distribution> distributions)
        {
            var assets = AssetBusiness.ListAssets();
            distributions.ForEach(c => c.Asset = assets.Single(a => a.Id == c.AssetId));
        }

        public List<Model.Portfolio.Distribution> ListByUserPortfolio(string email, int portfolioId)
        {
            var user = UserBusiness.GetValidUser(email);
            var followed = Task.Factory.StartNew(() => FollowBusiness.Get(user.Id, portfolioId));
            var portfolio = Task.Factory.StartNew(() => PortfolioBusiness.GetWithDetails(portfolioId));
            Task.WaitAll(followed, portfolio);

            var purchased = followed.Result != null;
            var owned = portfolio.Result != null && portfolio.Result.Advisor.UserId == user.Id;
            if (!purchased && !owned)
                throw new ArgumentException("Invalid portfolio distribution.");

            return ListByProjection(portfolio.Result.ProjectionId.Value);
        }

        public List<Model.Portfolio.Distribution> ListByProjection(int projectionId)
        {
            return List(new int[] { projectionId }).Select(c => new Model.Portfolio.Distribution()
            {
                Id = c.AssetId,
                Code = c.Asset.Code,
                Name = c.Asset.Name,
                Percentage = c.Percent,
                Type = (int)c.Asset.Type
            }).OrderByDescending(c => c.Percentage).ToList();
        }

        public void InsertMany(List<Auctus.DomainObjects.Portfolio.Distribution> distributions)
        {
            Data.InsertManyAsync(distributions);
        }
    }
}
