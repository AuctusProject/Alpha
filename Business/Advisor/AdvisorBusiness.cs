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
            advisor.Type = AdvisorType.Human.Value;
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

        public KeyValuePair<int, IEnumerable<Model.Portfolio>> ListRoboAdvisors(string email, int goalOptionId, int risk)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
            var goalOption = GoalOptionsBusiness.Get(goalOptionId);
            var riskType = RiskType.Get(risk, goalOption.Risk);
            var advisors = Data.ListRobosAvailable();
            var portfolios = Task.Factory.StartNew(() => PortfolioBusiness.List(advisors.Select(c => c.Id)));
            
            Task.WaitAll(portfolios);

            var portfolioQty = Task.Factory.StartNew(() => BuyBusiness.ListPortfoliosPurchases(portfolios.Result.SelectMany(c => c.Value.Select(x => x.Id))));

            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios.Result.SelectMany(c => c.Value))
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));

            Task.WaitAll(purchases, portfolioQty);
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

                var riskPriority = RiskType.GetRiskPriority(riskType);
                foreach(var r in riskPriority)
                {
                    var sameRisk = advisorPortfolios.Value.SingleOrDefault(c => c.Projection.RiskType == r);
                    if (sameRisk != null)
                    {
                        portfolioWithSameRisk.Add(PortfolioBusiness.FillPortfolioModel(sameRisk, advisor, user, purchases.Result, portfolioQty.Result));
                        break;
                    }
                }
            }
            var result = portfolioWithSameRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
            result.AddRange(portfolioWithLittleLowerRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithLittleHigherRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithLowerRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));
            result.AddRange(portfolioWithHigherRisk.OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent));

            return new KeyValuePair<int, IEnumerable<Model.Portfolio>>(riskType.Value, result);
        }
        
        public Model.Advisor ListDetails(string email, int advisorId)
        {
            User user = null;
            if (!string.IsNullOrEmpty(email))
                user = UserBusiness.GetValidUser(email);

            var advisor = GetWithDetail(advisorId);
            var owned = (user != null && user.Id == advisor.UserId);
            if (advisor.Type == AdvisorType.Robo.Value || (!advisor.Detail.Enabled && !owned))
                throw new ArgumentException("Invalid advisor.");

            Task<List<Buy>> purchases = null;
            if (user != null)
                purchases = Task.Factory.StartNew(() => BuyBusiness.ListUserAdvisorPurchases(user.Id, advisorId));

            var advisorQty = Task.Factory.StartNew(() => BuyBusiness.ListAdvisorsPurchases(new int[] { advisorId }));
            var portfolios = PortfolioBusiness.ListWithHistory(advisorId, !owned);
            var portfolioQty = Task.Factory.StartNew(() => BuyBusiness.ListPortfoliosPurchases(portfolios.Select(x => x.Id)));

            if (user != null)
                Task.WaitAll(purchases, advisorQty, portfolioQty);
            else
                Task.WaitAll(advisorQty, portfolioQty);
            
            return new Model.Advisor()
            {
                Id = advisor.Id,
                Name = advisor.Detail.Name,
                Description = advisor.Detail.Description,
                Owned = owned,
                Enabled = advisor.Detail.Enabled,
                PurchaseQuantity = advisorQty.Result.ContainsKey(advisor.Id) ? advisorQty.Result[advisor.Id] : 0,
                Portfolios = portfolios.Select(c => PortfolioBusiness.FillPortfolioModel(c, advisor, user, purchases?.Result, portfolioQty.Result)).
                    OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent).ToList()
            };
        }
    }
}
