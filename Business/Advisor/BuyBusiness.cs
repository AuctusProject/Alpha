using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Advisor
{
    public class BuyBusiness : BaseBusiness<Buy, BuyData>
    {
        public BuyBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Buy Create(string email, string address, int portfolioId, int days, int? goalOptionId = null, int? timeframe = null, 
            int? risk = null, double? targetAmount = null, double? startingAmount = null, double? monthlyContribution = null)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Address must be filled.");
            if (days <= 0)
                throw new ArgumentException("Invalid purchase days.");

            var user = UserBusiness.GetValidUser(email, address);
            if (user?.ConfirmationDate == null)
                throw new ArgumentException("User didn't confirmed e-mail.");
            var portfolio = PortfolioBusiness.GetWithDetails(portfolioId);

            if (portfolio == null || !portfolio.Detail.Enabled || !portfolio.Advisor.Detail.Enabled)
                throw new ArgumentException("Invalid portfolio.");
            if (portfolio.Advisor.UserId == user.Id)
                throw new ArgumentException("User is the advisor owner.");
           
            var purchases = ListPurchases(user.Id);
            if (purchases.Any(c => c.PortfolioId == portfolio.Id))
                throw new ArgumentException("Portfolio already bought.");

            Buy buy;
            using (var transaction = new TransactionalDapperCommand())
            {
                Goal goal = null;
                if (portfolio.Advisor.Type == AdvisorType.Robo.Value)
                {
                    if (!goalOptionId.HasValue || !timeframe.HasValue || !risk.HasValue || !startingAmount.HasValue || !monthlyContribution.HasValue)
                        throw new ArgumentException("Invalid goal data.");

                    goal = GoalBusiness.SetNew(user.Id, goalOptionId.Value, timeframe.Value, risk.Value, targetAmount, startingAmount.Value, monthlyContribution.Value);
                    transaction.Insert(goal);
                }
                var price = portfolio.Detail.Price.HasValue ? (Math.Floor(portfolio.Detail.Price.Value * (decimal)(1000000.0 * days) / (decimal)30.0) / (decimal)1000000.0) : (decimal?)null;
                buy = SetNew(days, price, portfolio.Id, portfolio.ProjectionId.Value, portfolio.Detail.Id, user.Id, goal?.Id);
                transaction.Insert(buy);
                var trans = TransactionBusiness.SetNew(user.Id);
                transaction.Insert(trans);
                var buyTrans = BuyTransactionBusiness.SetNew(buy.Id, trans.Id);
                transaction.Insert(buyTrans);
                buy.Goal = goal;
                buy.Portfolio = portfolio;
                buy.PortfolioDetail = portfolio.Detail;
                transaction.Commit();
            }
            return buy;
        }
        
        public Buy SetNew(int days, decimal? price, int portfolioId, int projectionId, int portfolioDetailId, int userId, int? goalId)
        {
            var buy = new Buy();
            buy.CreationDate = DateTime.UtcNow;
            buy.Days = days;
            buy.Price = price ?? 0;
            buy.PortfolioId = portfolioId;
            buy.ProjectionId = projectionId;
            buy.PortfolioDetailId = portfolioDetailId;
            buy.UserId = userId;
            buy.GoalId = goalId;
            return buy;
        }

        public List<Buy> ListPurchases(int userId)
        {
            return ListValidPurchases(Data.ListPurchases(userId));
        }

        public Buy Get(int id)
        {
            return Data.Get(id);
        }

        public Buy GetSimple(int id)
        {
            return Data.GetSimple(id);
        }

        public Buy Get(int userId, int portfolioId)
        {
            return Data.Get(userId, portfolioId);
        }

        public List<Buy> ListUserAdvisorPurchases(int userId, int advisorId)
        {
            return ListValidPurchases(Data.ListUserAdvisorPurchases(userId, advisorId));
        }

        public decimal? ListPortfolioPurchaseAmount(int portfolioId)
        {
            return Data.ListPortfolioPurchaseAmount(portfolioId);
        }

        public List<Buy> ListPendingEscrowResult()
        {
            return Data.ListPendingEscrowResult();
        }

        public Dictionary<int, int> ListAdvisorsPurchases(IEnumerable<int> advisorIds)
        {
            return Data.ListAdvisorsPurchases(advisorIds);
        }

        public Dictionary<int, int> ListPortfoliosPurchases(IEnumerable<int> portfolioIds)
        {
            return Data.ListPortfoliosPurchases(portfolioIds);
        }

        private List<Buy> ListValidPurchases(IEnumerable<Buy> purchases)
        {
            return purchases.Where(c => IsValidPurchase(c)).ToList();
        }

        public bool IsValidPurchase(Buy purchase)
        {
            return purchase != null && purchase.LastTransaction != null &&
                ((purchase.ExpirationDate.HasValue && purchase.ExpirationDate.Value >= DateTime.UtcNow.Date) ||
                 (!purchase.ExpirationDate.HasValue && purchase.LastTransaction.TransactionStatus != TransactionStatus.Cancel.Value
                    && purchase.LastTransaction.TransactionStatus != TransactionStatus.Fraud.Value));
        }
    }
}
