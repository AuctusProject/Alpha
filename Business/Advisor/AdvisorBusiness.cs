﻿using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Advisor
{
    public class AdvisorBusiness : BaseBusiness<DomainObjects.Advisor.Advisor, AdvisorData>
    {
        public int DefaultAdvisorId { get { return 1; } }

        public AdvisorBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public DomainObjects.Advisor.Advisor Create(string email, string name, string description, int period, double price)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (name.Length > 50)
                throw new ArgumentException("Name is too long.");

            var user = UserBusiness.GetValidUser(email);
            var advisor = new DomainObjects.Advisor.Advisor();
            using (var transaction = new TransactionalDapperCommand())
            {
                advisor.Name = name;
                transaction.Insert(advisor);
                var detail = AdvisorDetailBusiness.SetNew(advisor.Id, description, period, price, false);
                transaction.Insert(detail);
                advisor.Detail = detail;
                transaction.Commit();
            }
            return advisor;
        }

        public DomainObjects.Advisor.Advisor GetWithOwner(int id, string email)
        {
            return Data.GetWithOwner(id, email);
        }

        public DomainObjects.Advisor.Advisor GetWithDetail(int id)
        {
            var advisor = Data.GetWithDetail(id);
            if (advisor == null)
                throw new ArgumentException("Advisor cannot be found.");
            return advisor;
        }

        public IEnumerable<Model.Advisor> ListAvailable(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
            var advisors = Data.ListAvailable();
            var advisorsQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(advisors.Select(c => c.Id)));
            var portfolios = Task.Factory.StartNew(() => PortfolioBusiness.List(advisors.Select(c => c.Id)));

            Task.WaitAll(purchases, advisorsQty, portfolios);

            return advisors.Select(c => new Model.Advisor()
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Detail.Description,
                Period = c.Detail.Period,
                Price = c.Detail.Price,
                Purchased = purchases.Result.Any(x => x.AdvisorId == c.Id),
                PurchaseQuantity = advisorsQty.Result.ContainsKey(c.Id) ? advisorsQty.Result[c.Id] : 0,
                Projection = portfolios.Result[c.Id].Select(x => new Model.Advisor.RiskProjection()
                {
                    Risk = x.Risk,
                    ProjectionPercent = x.Projection.ProjectionValue,
                    OptimisticPercent= x.Projection.OptimisticProjection,
                    PessimisticPercent = x.Projection.PessimisticProjection
                }).ToList()
            });
        }

        public Model.Advisor ListDetails(string email, int advisorId)
        {
            var user = UserBusiness.GetValidUser(email);
            var advisor = Task.Factory.StartNew(() => GetWithDetail(advisorId));
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListUserAdvisorPurchases(user.Id, advisorId));
            var advisorQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(new int[] { advisorId }));
            var portfolios = PortfolioBusiness.ListWithHistory(advisorId);

            Task.WaitAll(advisor, purchases, advisorQty);

            var purchase = purchases.Result.SingleOrDefault(c => c.ExpirationDate > DateTime.UtcNow);
            var result = new Model.Advisor()
            {
                Id = advisor.Result.Id,
                Name = advisor.Result.Name,
                Description = advisor.Result.Detail.Description,
                Period = advisor.Result.Detail.Period,
                Price = advisor.Result.Detail.Price,
                Purchased = purchase != null,
                PurchaseQuantity = advisorQty.Result.ContainsKey(advisorId) ? advisorQty.Result[advisorId] : 0,
                Projection = portfolios.Select(x => new Model.Advisor.RiskProjection()
                {
                    Risk = x.Risk,
                    ProjectionPercent = x.Projection.ProjectionValue,
                    OptimisticPercent = x.Projection.OptimisticProjection,
                    PessimisticPercent = x.Projection.PessimisticProjection
                }).ToList(),
                Detail = new Model.Advisor.Details()
                {
                    PurchaseInfo = new Model.Advisor.Purchase()
                    {
                        QtyAlreadyPurchased = purchases.Result.Count,
                        ExpirationTime = purchase != null ? purchase.ExpirationDate : (DateTime?)null,
                        Risk = purchase != null ? purchase.Goal.Risk : (int?)null
                    }
                }
            };
            result.Detail.PortfolioHistory = GetPortfolioHistory(portfolios);
            return result;
        }

        public List<Model.Advisor.History> GetPortfolioHistory(IEnumerable<DomainObjects.Portfolio.Portfolio> portfolios)
        {
            List<Model.Advisor.History> result = new List<Model.Advisor.History>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
            {
                result.Add(new Model.Advisor.History()
                {
                    Risk = portfolio.Risk,
                    TotalDays = portfolio.PortfolioHistory.Count,
                    LastDay = GetHistoryResult(1, portfolio.PortfolioHistory),
                    Last7Days = GetHistoryResult(7, portfolio.PortfolioHistory),
                    Last30Days = GetHistoryResult(30, portfolio.PortfolioHistory),
                    AllDays = GetHistoryResult((int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolio.PortfolioHistory.Min(c => c.Date)).TotalDays) + 1, portfolio.PortfolioHistory),
                    Histogram = GetHistogram(portfolio.PortfolioHistory)
                });
            }
            return result;
        }

        private List<Model.Advisor.Distribution> GetHistogram(IEnumerable<PortfolioHistory> portfolioHistory)
        {
            var values = portfolioHistory.OrderBy(c => c.RealValue).Select(c => c.RealValue);
            var minValue = values.First();
            var maxValue = values.Last();
            var difference = maxValue - minValue;
            double rangeGroup;
            if (difference == 0)
                rangeGroup = 1;
            else
                rangeGroup = difference / (values.Count() > 75 ? 15.0 : Math.Floor(values.Count() / 5.0));

            minValue = minValue - (rangeGroup / 1.5);
            maxValue = maxValue + (rangeGroup / 1.5);

            List<Model.Advisor.Distribution> result = new List<Model.Advisor.Distribution>();
            for (double i = minValue; i <= maxValue; i = i + rangeGroup)
            {
                result.Add(new Model.Advisor.Distribution()
                {
                    GreaterOrEqual = i,
                    Lesser = i + rangeGroup,
                    Quantity = portfolioHistory.Count(c => c.RealValue >= i && c.RealValue < (i + rangeGroup))
                });
            }
            return result;
        }

        private Model.Advisor.HistoryResult GetHistoryResult(int days, IEnumerable<PortfolioHistory> portfolioHistory)
        {
            var history = portfolioHistory.Where(c => c.Date >= DateTime.UtcNow.AddDays(-days).Date);
            return !history.Any() ? null : new Model.Advisor.HistoryResult()
            {
                Value = (history.Select(c => 1 + (c.RealValue / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                ExpectedValue = (history.Select(c => 1 + (c.ProjectionValue / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                OptimisticExpectation = history.Any(c => !c.OptimisticProjectionValue.HasValue) ? (double?)null : 
                                        (history.Select(c => 1 + (c.OptimisticProjectionValue.Value / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                PessimisticExpectation = history.Any(c => !c.PessimisticProjectionValue.HasValue) ? (double?)null :
                                        (history.Select(c => 1 + (c.PessimisticProjectionValue.Value / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                HitPercentage = (double)history.Count(c => c.RealValue >= c.ProjectionValue) / (double)history.Count()
            };
        }
    }
}