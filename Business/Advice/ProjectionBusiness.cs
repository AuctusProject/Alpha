using Auctus.DataAccess.Advice;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advice;
using Auctus.Model;
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

            return Create(portfolio, projectionValue, optimisticProjection, pessimisticProjection, distribution);
        }

        internal Projection Create(Portfolio portfolio, double projectionValue, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var projection = SetNew(portfolio.Id, projectionValue, optimisticProjection, pessimisticProjection);
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

        public Projection Get(int projectionId)
        {
            return Data.Get(projectionId);
        }
        
        public Projections GetProjections(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = BuyBusiness.ListPurchasesComplete(user.Id);
            user.Goal = GoalBusiness.GetCurrent(user.Id);
            var projections = new Projections();
            projections.CurrentGoal = new Projections.Goal()
            {
                Risk = RiskType.Get(user.Goal.Risk, user.Goal.GoalOption.Risk).Value,
                Timeframe = user.Goal.Timeframe,
                MonthlyContribution = user.Goal.MonthlyContribution,
                StartingAmount = user.Goal.StartingAmount,
                TargetAmount = user.Goal.TargetAmount
            };
            projections.Purchases = new List<Projections.Purchase>();
            foreach(Buy purchase in purchases)
            {
                projections.Purchases.Add(new Projections.Purchase()
                {
                    PurchaseDate = purchase.CreationDate,
                    ExpirationDate = purchase.ExpirationDate,
                    AdvisorId = purchase.AdvisorId,
                    Description = purchase.Advisor.Detail.Description,
                    Name = purchase.Advisor.Name,
                    Period = purchase.Advisor.Detail.Period,
                    Price = purchase.Advisor.Detail.Price,
                    Goal = new Projections.Goal()
                    {
                        MonthlyContribution = purchase.Goal.MonthlyContribution,
                        StartingAmount = purchase.Goal.StartingAmount,
                        Timeframe = purchase.Goal.Timeframe,
                        TargetAmount = purchase.Goal.TargetAmount,
                        Risk = RiskType.Get(purchase.Goal.Risk, purchase.Goal.GoalOption.Risk).Value
                    },
                    ProjectionData = new Projections.Projection()
                    {
                        Risk = purchase.Projection.Portfolio.Risk,
                        ProjectionPercent = purchase.Projection.ProjectionValue,
                        OptimisticPercent = purchase.Projection.OptimisticProjection,
                        PessimisticPercent = purchase.Projection.PessimisticProjection,
                        CurrentOptimisticValue = GetProjectionValue(purchase.Projection.OptimisticProjection, user.Goal),
                        CurrentPessimisticValue = GetProjectionValue(purchase.Projection.PessimisticProjection, user.Goal),
                        CurrentProjectionValue = GetProjectionValue(purchase.Projection.ProjectionValue, user.Goal),
                        PurchasedOptimisticValue = GetProjectionValue(purchase.Projection.OptimisticProjection, purchase.Goal),
                        PurchasedPessimisticValue = GetProjectionValue(purchase.Projection.PessimisticProjection, purchase.Goal),
                        PurchasedProjectionValue = GetProjectionValue(purchase.Projection.ProjectionValue, purchase.Goal)
                    }
                });
            }
            return projections;
        }

        private double? GetProjectionValue(double? percent, Goal goal)
        {
            if (percent > 0 && goal.Timeframe > 0 && (goal.StartingAmount > 0 || goal.MonthlyContribution > 0))
            {
                var start = !goal.StartingAmount.HasValue ? 0 : goal.StartingAmount.Value * Math.Pow((1 + percent.Value / 100), goal.Timeframe.Value);
                var monthly = !goal.MonthlyContribution.HasValue ? 0 : goal.StartingAmount.HasValue ?
                    goal.MonthlyContribution.Value * ((Math.Pow((1 + percent.Value / 100), goal.Timeframe.Value) - 1) / (percent.Value / 100)) :
                    goal.MonthlyContribution.Value * (((Math.Pow((1 + percent.Value / 100), goal.Timeframe.Value + 1) - 1) / (percent.Value / 100)) - 1);
                return start + monthly;
            }
            return (double?)null;
        }
    }
}
