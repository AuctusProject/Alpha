using Auctus.DataAccess.Advice;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Advice
{
    public class ProjectionBusiness : BaseBusiness<Projection, ProjectionData>
    {
        public ProjectionBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Projection Create(string email, int portfolioId, double projectionValue, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var portfolio = PortfolioBusiness.GetValidByOwner(email, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            var projection = SetNew(portfolioId, projectionValue, optimisticProjection, pessimisticProjection);
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(projection);
                var distributions = DistributionBusiness.SetNew(projection.Id, distribution);
                foreach (Distribution dist in distributions)
                    transaction.Insert(dist);

                portfolio.ProjectionId = projection.Id;
                transaction.Update(portfolio);
                projection.Distribution = distributions;
                transaction.Commit();
            }
            return projection;
        }

        public Projection SetNew(int portfolioId, double projectionValue, double? optimisticProjection, double? pessimisticProjection)
        {
            if (projectionValue <= 0)
                throw new ArgumentException("Invalid projection value.");
            if (optimisticProjection.HasValue && optimisticProjection.Value < projectionValue)
                throw new ArgumentException("Invalid optimistic projection value.");
            if (pessimisticProjection.HasValue && (pessimisticProjection.Value < 0 || pessimisticProjection.Value > projectionValue))
                throw new ArgumentException("Invalid pessimistic projection value.");

            var projection = new Projection();
            projection.PortfolioId = portfolioId;
            projection.Date = DateTime.UtcNow;
            projection.ProjectionValue = projectionValue;
            projection.OptimisticProjection = optimisticProjection;
            projection.PessimisticProjection = pessimisticProjection;
            return projection;
        }
    }
}
