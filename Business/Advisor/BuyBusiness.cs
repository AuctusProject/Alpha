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
            var portfolio = PortfolioBusiness.GetWithDetail(portfolioId);

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
                if (portfolio.Advisor.Type == AdvisorType.Robo)
                {
                    goal = GoalBusiness.SetNew(user.Id, goalOptionId.Value, timeframe.Value, risk.Value, targetAmount, startingAmount.Value, monthlyContribution.Value);
                    transaction.Insert(goal);
                }
                buy = SetNew(days, (portfolio.Detail.Price * days / 30.0), portfolio.Id, portfolio.ProjectionId.Value, portfolio.Detail.Id, user.Id, goal?.Id);
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
        
        public Buy SetNew(int days, double price, int portfolioId, int projectionId, int portfolioDetailId, int userId, int? goalId)
        {
            var buy = new Buy();
            buy.CreationDate = DateTime.UtcNow;
            buy.Days = days;
            buy.Price = price;
            buy.PortfolioId = portfolioId;
            buy.ProjectionId = projectionId;
            buy.PortfolioDetailId = portfolioDetailId;
            buy.UserId = userId;
            buy.GoalId = goalId;
            return buy;
        }

        public List<Buy> ListPurchases(int userId)
        {
            var purchases = Data.ListPurchases(userId);
            return purchases.Where(c => (c.ExpirationDate.HasValue && c.ExpirationDate.Value >= DateTime.UtcNow) ||
                                    (!c.ExpirationDate.HasValue && c.LastTransaction.TransactionStatus != TransactionStatus.Cancel)).ToList();
        }

        public Buy Get(int id)
        {
            return Data.Get(id);
        }

        public List<Buy> ListUserAdvisorPurchases(int userId, int advisorId)
        {
            return Data.ListUserAdvisorPurchases(userId, advisorId);
        }

        public Dictionary<int, int> ListAdvisorsPurchases(IEnumerable<int> advisorIds)
        {
            return Data.ListAdvisorsPurchases(advisorIds);
        }

        public Dictionary<int, int> ListPortfoliosPurchases(IEnumerable<int> portfolioIds)
        {
            return Data.ListPortfoliosPurchases(portfolioIds);
        }

        //public List<Buy> ListPurchasesWithPortfolio(int userId)
        //{
        //    return Data.ListPurchasesWithPortfolio(userId);
        //}

        //public List<Buy> ListPurchasesComplete(int userId)
        //{
        //    var purchases = Data.ListPurchasesComplete(userId);
        //    var options = GoalOptionsBusiness.List();
        //    purchases.ForEach(c => c.Goal.GoalOption = options.Single(o => o.Id == c.Goal.GoalOptionId));
        //    return purchases;
        //}
    }
}
