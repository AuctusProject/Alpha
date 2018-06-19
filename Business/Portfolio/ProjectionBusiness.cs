using Auctus.DataAccess.Core;
using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.DomainObjects.Portfolio;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Portfolio
{
    public class ProjectionBusiness : BaseBusiness<Projection, ProjectionData>
    {
        public ProjectionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Projection Create(string email, int portfolioId, double projectionValue, RiskType risk, double? optimisticProjection,
            double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = PortfolioBusiness.GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            return Create(portfolio, projectionValue, risk, optimisticProjection, pessimisticProjection, distribution);
        }

        internal Projection Create(DomainObjects.Portfolio.Portfolio portfolio, double? projectionValue, RiskType risk, 
            double? optimisticProjection, double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var projection = SetNew(portfolio.Id, projectionValue, risk, optimisticProjection, pessimisticProjection);
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(projection);
                var distributions = DistributionBusiness.SetNew(projection.Id, portfolio.Id, distribution, projection.Date);
                DistributionBusiness.InsertMany(distributions);

                portfolio.ProjectionId = projection.Id;
                transaction.Update(portfolio);
                projection.Distribution = distributions;
                transaction.Commit();
            }
            return projection;
        }

        public Projection SetNew(int portfolioId, double? projectionValue, RiskType risk, double? optimisticProjection, double? pessimisticProjection)
        {
            var projection = new Projection();
            projection.PortfolioId = portfolioId;
            projection.Date = DateTime.UtcNow;
            projection.Risk = risk?.Value;
            projection.ProjectionValue = projectionValue;
            projection.OptimisticProjectionValue = optimisticProjection;
            projection.PessimisticProjectionValue = pessimisticProjection;
            return projection;
        }

        public Projection Get(int projectionId)
        {
            return Data.Get(projectionId);
        }

        private Model.Portfolio.Result GetProjectionResult(Goal goal, Projection projection)
        {
            Model.Portfolio.Result result = new Model.Portfolio.Result();
            if (goal.Timeframe > 0 && (goal.StartingAmount > 0 || goal.MonthlyContribution > 0))
            {
                if (projection.OptimisticProjectionValue > 0)
                    result.OptimisticValue = GetStartProjectionValue(goal, projection.OptimisticProjectionValue.Value) + GetMonthlyProjectionValue(goal, projection.OptimisticProjectionValue.Value);
                if (projection.PessimisticProjectionValue > 0)
                    result.PessimisticValue = GetStartProjectionValue(goal, projection.PessimisticProjectionValue.Value) + GetMonthlyProjectionValue(goal, projection.PessimisticProjectionValue.Value);

                if (projection.ProjectionValue > 0)
                {
                    var start = GetStartProjectionValue(goal, projection.ProjectionValue);
                    var monthly = GetMonthlyProjectionValue(goal, projection.ProjectionValue);
                    result.ProjectionValue = start + monthly;
                    if (goal.TargetAmount > 0 && monthly.HasValue && start.HasValue)
                    {
                        result.Reached = goal.TargetAmount.Value <= result.ProjectionValue.Value;
                        result.Difference = Math.Abs(goal.TargetAmount.Value - result.ProjectionValue.Value);
                        result.NewStartingAmount = Math.Abs(goal.TargetAmount.Value - monthly.Value) / GetStartProjectionInterestRate(goal.Timeframe, projection.ProjectionValue.Value);
                        result.NewMonthlyContribution = Math.Abs(goal.TargetAmount.Value - start.Value) /
                            (goal.StartingAmount > 0 ?
                            GetMonthlyProjectionExpiredInterestRate(goal.Timeframe, projection.ProjectionValue.Value) :
                            GetMonthlyProjectionAntecipatedInterestRate(goal.Timeframe, projection.ProjectionValue.Value));
                    }
                }
            }
            return result;
        }

        private double GetStartProjectionInterestRate(int period, double percent)
        {
            return Math.Pow((1 + percent / 100), period);
        }

        private double GetMonthlyProjectionAntecipatedInterestRate(int period, double percent)
        {
            return ((Math.Pow((1 + percent / 100), period + 1) - 1) / (percent / 100)) - 1;
        }

        private double GetMonthlyProjectionExpiredInterestRate(int period, double percent)
        {
            return (Math.Pow((1 + percent / 100), period) - 1) / (percent / 100);
        }

        private double? GetStartProjectionValue(Goal goal, double? percent)
        {
            if (!percent.HasValue)
                return null;
            return goal.StartingAmount * GetStartProjectionInterestRate(goal.Timeframe, percent.Value);
        }

        private double? GetMonthlyProjectionValue(Goal goal, double? percent)
        {
            if (!percent.HasValue)
                return null;
            return goal.StartingAmount > 0 ?
                    goal.MonthlyContribution * GetMonthlyProjectionExpiredInterestRate(goal.Timeframe, percent.Value) :
                    goal.MonthlyContribution * GetMonthlyProjectionAntecipatedInterestRate(goal.Timeframe, percent.Value);
        }
    }
}
