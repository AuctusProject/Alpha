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

        internal Projection Create(DomainObjects.Portfolio.Portfolio portfolio, double projectionValue, RiskType risk, 
            double? optimisticProjection, double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var projection = SetNew(portfolio.Id, projectionValue, risk, optimisticProjection, pessimisticProjection);
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

        public Projection SetNew(int portfolioId, double projectionValue, RiskType risk, double? optimisticProjection, double? pessimisticProjection)
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
            projection.Risk = risk.Value;
            projection.ProjectionValue = projectionValue;
            projection.OptimisticProjectionValue = optimisticProjection;
            projection.PessimisticProjectionValue = pessimisticProjection;
            return projection;
        }

        public Projection Get(int projectionId)
        {
            return Data.Get(projectionId);
        }
        
        //public Projections GetProjections(string email)
        //{
        //    var user = UserBusiness.GetValidUser(email);
        //    var purchases = BuyBusiness.ListPurchasesComplete(user.Id);
        //    user.Goal = GoalBusiness.GetCurrent(user.Id);
        //    var projections = new Projections();
        //    projections.CurrentGoal = new Projections.Goal()
        //    {
        //        Risk = RiskType.Get(user.Goal.Risk, user.Goal.GoalOption.Risk).Value,
        //        Timeframe = user.Goal.Timeframe,
        //        MonthlyContribution = user.Goal.MonthlyContribution,
        //        StartingAmount = user.Goal.StartingAmount,
        //        TargetAmount = user.Goal.TargetAmount
        //    };
        //    projections.Purchases = new List<Projections.Purchase>();
        //    foreach(Buy purchase in purchases)
        //    {
        //        projections.Purchases.Add(new Projections.Purchase()
        //        {
        //            PurchaseDate = purchase.CreationDate,
        //            ExpirationDate = purchase.ExpirationDate,
        //            AdvisorId = purchase.AdvisorId,
        //            Description = purchase.Advisor.Detail.Description,
        //            Name = purchase.Advisor.Name,
        //            Period = purchase.Advisor.Detail.Period,
        //            Price = purchase.Advisor.Detail.Price,
        //            Goal = new Projections.Goal()
        //            {
        //                MonthlyContribution = purchase.Goal.MonthlyContribution,
        //                StartingAmount = purchase.Goal.StartingAmount,
        //                Timeframe = purchase.Goal.Timeframe,
        //                TargetAmount = purchase.Goal.TargetAmount,
        //                Risk = RiskType.Get(purchase.Goal.Risk, purchase.Goal.GoalOption.Risk).Value
        //            },
        //            ProjectionData = new Projections.Projection()
        //            {
        //                Risk = purchase.Projection.Portfolio.Risk,
        //                ProjectionPercent = purchase.Projection.ProjectionValue,
        //                OptimisticPercent = purchase.Projection.OptimisticProjection,
        //                PessimisticPercent = purchase.Projection.PessimisticProjection,
        //                Current = GetProjectionResult(user.Goal, purchase.Projection),
        //                Purchased = GetProjectionResult(purchase.Goal, purchase.Projection)
        //            }
        //        });
        //    }
        //    return projections;
        //}

        private Projections.Result GetProjectionResult(Goal goal, Projection projection)
        {
            Projections.Result result = new Projections.Result();
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
                    if (goal.TargetAmount > 0)
                    {
                        result.Reached = goal.TargetAmount.Value <= result.ProjectionValue.Value;
                        result.Difference = Math.Abs(goal.TargetAmount.Value - result.ProjectionValue.Value);
                        result.NewStartingAmount = Math.Abs(goal.TargetAmount.Value - monthly) / GetStartProjectionInterestRate(goal.Timeframe, projection.ProjectionValue);
                        result.NewMonthlyContribution = Math.Abs(goal.TargetAmount.Value - start) /
                            (goal.StartingAmount > 0 ?
                            GetMotnhlyProjectionExpiredInterestRate(goal.Timeframe, projection.ProjectionValue) :
                            GetMotnhlyProjectionAntecipatedInterestRate(goal.Timeframe, projection.ProjectionValue));
                    }
                }
            }
            return result;
        }

        private double GetStartProjectionInterestRate(int period, double percent)
        {
            return Math.Pow((1 + percent / 100), period);
        }

        private double GetMotnhlyProjectionAntecipatedInterestRate(int period, double percent)
        {
            return ((Math.Pow((1 + percent / 100), period + 1) - 1) / (percent / 100)) - 1;
        }

        private double GetMotnhlyProjectionExpiredInterestRate(int period, double percent)
        {
            return (Math.Pow((1 + percent / 100), period) - 1) / (percent / 100);
        }

        private double GetStartProjectionValue(Goal goal, double percent)
        {
            return goal.StartingAmount * GetStartProjectionInterestRate(goal.Timeframe, percent);
        }

        private double GetMonthlyProjectionValue(Goal goal, double percent)
        {
            return goal.StartingAmount > 0 ?
                    goal.MonthlyContribution * GetMotnhlyProjectionExpiredInterestRate(goal.Timeframe, percent) :
                    goal.MonthlyContribution * GetMotnhlyProjectionAntecipatedInterestRate(goal.Timeframe, percent);
        }
    }
}
