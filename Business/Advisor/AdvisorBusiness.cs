using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
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

namespace Auctus.Business.Advisor
{
    public class AdvisorBusiness : BaseBusiness<DomainObjects.Advisor.Advisor, AdvisorData>
    {
        public int DefaultAdvisorId { get { return 1; } }

        public AdvisorBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public DomainObjects.Advisor.Advisor Create(string email, string name, string description)
        {
            var user = UserBusiness.GetValidUser(email);
            var advisor = SimpleGetByOwner(user.Id);
            if (advisor != null)
                throw new ArgumentException("Advisor already registered.");

            advisor = new DomainObjects.Advisor.Advisor();
            advisor.UserId = user.Id;
            advisor.Type = AdvisorType.Human;
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(advisor);
                var detail = AdvisorDetailBusiness.SetNew(advisor.Id, name, description, false);
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

        public DomainObjects.Advisor.Advisor SimpleGetByOwner(int userId)
        {
            return Data.SimpleGetByOwner(userId);
        }

        public DomainObjects.Advisor.Advisor GetWithDetail(int id)
        {
            var advisor = Data.GetWithDetail(id);
            if (advisor == null)
                throw new ArgumentException("Advisor cannot be found.");
            return advisor;
        }

        public IEnumerable<Model.Portfolio> ListRoboAdvisors(string email, int goalOptionId, int risk)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
            var goalOption = GoalOptionsBusiness.Get(goalOptionId);
            var riskType = RiskType.Get(risk, goalOption.Risk);
            var advisors = Data.ListRobosAvailable();
            var advisorsQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(advisors.Select(c => c.Id)));
            var portfolios = Task.Factory.StartNew(() => PortfolioBusiness.List(advisors.Select(c => c.Id)));
            
            Task.WaitAll(portfolios);

            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios.Result.SelectMany(c => c.Value))
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));

            Task.WaitAll(purchases, advisorsQty);
            Task.WaitAll(histories.ToArray());

            List<Model.Portfolio> portfolioWithSameRisk = new List<Model.Portfolio>();
            List<Model.Portfolio> portfolioWithLittleLowerRisk = new List<Model.Portfolio>();
            List<Model.Portfolio> portfolioWithLittleHigherRisk = new List<Model.Portfolio>();
            List<Model.Portfolio> portfolioWithLowerRisk = new List<Model.Portfolio>();
            List<Model.Portfolio> portfolioWithHigherRisk = new List<Model.Portfolio>();
            foreach (KeyValuePair<int, List<DomainObjects.Portfolio.Portfolio>> advisorPortfolios in portfolios.Result)
            {
                var advisor = advisors.Single(c => c.Id == advisorPortfolios.Key);
                advisorPortfolios.Value.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());

                var sameRisk = advisorPortfolios.Value.SingleOrDefault(c => c.Projection.RiskType == riskType);
                if (sameRisk != null)
                    portfolioWithSameRisk.Add(FillPortfolioModel(sameRisk, advisor, user, purchases.Result, advisorsQty.Result));
                else
                {
                    var littleLower = advisorPortfolios.Value.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value - 1));
                    if (littleLower != null)
                        portfolioWithLittleLowerRisk.Add(FillPortfolioModel(littleLower, advisor, user, purchases.Result, advisorsQty.Result));
                    else
                    {
                        var littleHigher = advisorPortfolios.Value.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value + 1));
                        if (littleHigher != null)
                            portfolioWithLittleHigherRisk.Add(FillPortfolioModel(littleHigher, advisor, user, purchases.Result, advisorsQty.Result));
                        else
                        {
                            var lower = advisorPortfolios.Value.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value - 2));
                            if (lower != null)
                                portfolioWithLowerRisk.Add(FillPortfolioModel(lower, advisor, user, purchases.Result, advisorsQty.Result));
                            else
                                portfolioWithHigherRisk.Add(FillPortfolioModel(
                                    advisorPortfolios.Value.Single(c => c.Projection.RiskType.Value == (riskType.Value + 2)), 
                                    advisor, user, purchases.Result, advisorsQty.Result));
                        }
                    }
                }
            }

            var result = portfolioWithSameRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
            result.AddRange(portfolioWithLittleLowerRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithLittleHigherRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithLowerRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithHigherRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));

            return result;
        }

        private Model.Portfolio FillPortfolioModel(DomainObjects.Portfolio.Portfolio portfolio, DomainObjects.Advisor.Advisor advisor, User user,
            List<Buy> purchases, Dictionary<int, int> purchasesQty)
        {
            return new Model.Portfolio()
            {
                Id = portfolio.Id,
                Name = portfolio.Detail.Name,
                Description = portfolio.Detail.Description,
                Price = portfolio.Detail.Price,
                AdvisorId = portfolio.AdvisorId,
                AdvisorDescription = advisor.Detail.Description,
                AdvisorName = advisor.Detail.Name,
                Risk = portfolio.Projection.Risk,
                ProjectionPercent = portfolio.Projection.ProjectionValue,
                OptimisticPercent = portfolio.Projection.OptimisticProjection,
                PessimisticPercent = portfolio.Projection.PessimisticProjection,
                Owned = advisor.UserId == user.Id,
                Purchased = purchases.Any(x => x.PortfolioId == portfolio.Id),
                Enabled = true,
                PurchaseQuantity = purchasesQty.ContainsKey(portfolio.Id) ? purchasesQty[portfolio.Id] : 0,
                TotalDays = portfolio.PortfolioHistory.Count,
                LastDay = PortfolioHistoryBusiness.GetHistoryResult(1, portfolio.PortfolioHistory),
                Last7Days = PortfolioHistoryBusiness.GetHistoryResult(7, portfolio.PortfolioHistory),
                Last30Days = PortfolioHistoryBusiness.GetHistoryResult(30, portfolio.PortfolioHistory),
                AllDays = PortfolioHistoryBusiness.GetHistoryResult((int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolio.PortfolioHistory.Min(x => x.Date)).TotalDays) + 1, portfolio.PortfolioHistory)
            };
        }

        //public Model.Advisor ListDetails(string email, int advisorId)
        //{
        //    var user = UserBusiness.GetValidUser(email);
        //    var advisor = Task.Factory.StartNew(() => GetWithDetail(advisorId));
        //    var purchases = Task.Factory.StartNew(() => BuyBusiness.ListUserAdvisorPurchases(user.Id, advisorId));
        //    var advisorQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(new int[] { advisorId }));
        //    var portfolios = PortfolioBusiness.ListWithHistory(advisorId);

        //    Task.WaitAll(advisor, purchases, advisorQty);

        //    var purchase = purchases.Result.SingleOrDefault(c => c.ExpirationDate > DateTime.UtcNow);
        //    var result = new Model.Advisor()
        //    {
        //        Id = advisor.Result.Id,
        //        Name = advisor.Result.Name,
        //        Description = advisor.Result.Detail.Description,
        //        Period = advisor.Result.Detail.Period,
        //        Price = advisor.Result.Detail.Price,
        //        Purchased = purchase != null,
        //        PurchaseQuantity = advisorQty.Result.ContainsKey(advisorId) ? advisorQty.Result[advisorId] : 0,
        //        Projection = portfolios.Select(x => new Model.Advisor.RiskProjection()
        //        {
        //            Risk = x.Risk,
        //            ProjectionPercent = x.Projection.ProjectionValue,
        //            OptimisticPercent = x.Projection.OptimisticProjection,
        //            PessimisticPercent = x.Projection.PessimisticProjection
        //        }).ToList(),
        //        Detail = new Model.Advisor.Details()
        //        {
        //            PurchaseInfo = new Model.Advisor.Purchase()
        //            {
        //                QtyAlreadyPurchased = purchases.Result.Count,
        //                ExpirationTime = purchase != null ? purchase.ExpirationDate : (DateTime?)null,
        //                Risk = purchase != null ? purchase.Goal.Risk : (int?)null
        //            }
        //        }
        //    };
        //    result.Detail.PortfolioHistory = GetPortfolioHistory(portfolios);
        //    return result;
        //}

        //public List<Model.Advisor.History> GetPortfolioHistory(IEnumerable<DomainObjects.Portfolio.Portfolio> portfolios)
        //{
        //    List<Model.Advisor.History> result = new List<Model.Advisor.History>();
        //    foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
        //    {
        //        result.Add(new Model.Advisor.History()
        //        {
        //            Risk = portfolio.Risk,
        //            TotalDays = portfolio.PortfolioHistory.Count,
        //            LastDay = GetHistoryResult(1, portfolio.PortfolioHistory),
        //            Last7Days = GetHistoryResult(7, portfolio.PortfolioHistory),
        //            Last30Days = GetHistoryResult(30, portfolio.PortfolioHistory),
        //            AllDays = GetHistoryResult((int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolio.PortfolioHistory.Min(c => c.Date)).TotalDays) + 1, portfolio.PortfolioHistory),
        //            Histogram = GetHistogram(portfolio.PortfolioHistory)
        //        });
        //    }
        //    return result;
        //}
    }
}
