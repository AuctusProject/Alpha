using Auctus.DataAccess.Core;
using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Portfolio;
using Auctus.DomainObjects.Asset;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auctus.DomainObjects.Advisor;

namespace Auctus.Business.Portfolio
{
    public class PortfolioBusiness : BaseBusiness<DomainObjects.Portfolio.Portfolio, PortfolioData>
    {
        public PortfolioBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public DomainObjects.Portfolio.Portfolio Create(string email, int advisorId, double price, string name, string description, 
            double projectionValue, double? optimisticProjection, double? pessimisticProjection, Dictionary<int, double> distribution)
        {
            var advisor = AdvisorBusiness.GetWithOwner(advisorId, email);
            if (advisor == null)
                throw new ArgumentException("Invalid advisor.");

            var risk = GetRisk(projectionValue, distribution);
            var advisorDetail = AdvisorDetailBusiness.GetForAutoEnabled(advisorId);

            var portfolio = new DomainObjects.Portfolio.Portfolio();
            portfolio.AdvisorId = advisorId;
            portfolio.CreationDate = DateTime.UtcNow;
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(portfolio);
                var detail = PortfolioDetailBusiness.SetNew(portfolio.Id, price, name, description, true);
                transaction.Insert(detail);
                var projection = ProjectionBusiness.SetNew(portfolio.Id, projectionValue, risk, optimisticProjection, pessimisticProjection);
                transaction.Insert(projection);
                var distributions = DistributionBusiness.SetNew(projection.Id, distribution);
                foreach (Distribution dist in distributions)
                    transaction.Insert(dist);

                portfolio.ProjectionId = projection.Id;
                transaction.Update(portfolio);
                if (advisorDetail != null)
                {
                    advisorDetail = AdvisorDetailBusiness.SetNew(advisorId, advisorDetail.Name, advisorDetail.Description, true);
                    transaction.Insert(advisorDetail);
                }
                projection.Distribution = distributions;
                portfolio.Projection = projection;
                portfolio.Detail = detail;
                transaction.Commit();
            }
            return portfolio;
        }

        public DomainObjects.Portfolio.Portfolio Update(string email, int portfolioId, double price, string name, string description)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            portfolio.Detail = PortfolioDetailBusiness.Create(portfolioId, price, name, description, true);
            return portfolio;
        }

        internal Projection GetProjectionAtDate(DateTime date, DomainObjects.Portfolio.Portfolio portfolio)
        {
            return portfolio.Projections.Where(p => p.Date < date).OrderByDescending(p => p.Date).FirstOrDefault();
        }

        public void Disable(string email, int portfolioId)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            if (!List(portfolio.AdvisorId).Any(c => c.Id != portfolio.Id))
                throw new ArgumentException("Unique advisor's portfolio cannot be disabled.");

            PortfolioDetailBusiness.Create(portfolioId, portfolio.Detail.Price, portfolio.Detail.Name, portfolio.Detail.Description, false);
        }

        public RiskType GetRisk(double projectionValue, Dictionary<int, double> distribution)
        {
            IEnumerable<DomainObjects.Asset.Asset> assets = AssetBusiness.ListAssets().Where(c => distribution.ContainsKey(c.Id));
            double cryptoAssetsPercentage = assets.Count(c => c.Type == AssetType.Crypto) / assets.Count() * 100.0;
            if (cryptoAssetsPercentage == 0)
            {
                if (projectionValue >= 3)
                    return RiskType.VeryHigh;
                else if (projectionValue >= 2)
                    return RiskType.High;
                else if (projectionValue >= 1)
                    return RiskType.Medium;
                else if (projectionValue >= 0.5)
                    return RiskType.Low;
                else
                    return RiskType.VeryLow;
            }
            else if (cryptoAssetsPercentage <= 10)
            {
                if (projectionValue >= 2)
                    return RiskType.VeryHigh;
                else if (projectionValue >= 1)
                    return RiskType.High;
                else if (projectionValue >= 0.5)
                    return RiskType.Medium;
                else
                    return RiskType.Low;
            }
            else if (cryptoAssetsPercentage <= 30)
            {
                if (projectionValue >= 1)
                    return RiskType.VeryHigh;
                else if (projectionValue >= 0.5)
                    return RiskType.High;
                else
                    return RiskType.Medium;
            }
            else if (cryptoAssetsPercentage <= 50)
            {
                if (projectionValue >= 0.5)
                    return RiskType.VeryHigh;
                else
                    return RiskType.High;
            }
            else
                return RiskType.VeryHigh;
        }

        public DomainObjects.Portfolio.Portfolio GetWithDetails(int portfolioId)
        {
            return ListWithDetails(new int[] { portfolioId }).SingleOrDefault();
        }

        public List<DomainObjects.Portfolio.Portfolio> ListWithDetails(IEnumerable<int> portfolioIds)
        {
            return Data.ListWithDetails(portfolioIds);
        }

        public DomainObjects.Portfolio.Portfolio GetByRisk(int roboAdvisorId, RiskType riskType)
        {
            string defaultPortfoliosKey = "DefaultPortfolios";
            List<DomainObjects.Portfolio.Portfolio> portfolios = null;
            if (roboAdvisorId == AdvisorBusiness.DefaultAdvisorId)
                portfolios = MemoryCache.Get<List<DomainObjects.Portfolio.Portfolio>>(defaultPortfoliosKey);
            if (portfolios == null)
            {
                portfolios = List(roboAdvisorId).Where(c => c.Advisor.Type == DomainObjects.Advisor.AdvisorType.Robo).ToList();
                if (roboAdvisorId == AdvisorBusiness.DefaultAdvisorId)
                    MemoryCache.Set<List<DomainObjects.Portfolio.Portfolio>>(defaultPortfoliosKey, portfolios);
            }

            if (portfolios.Count == 1)
                return portfolios.First();

            var sameRisk = portfolios.SingleOrDefault(c => c.Projection.RiskType == riskType);
            if (sameRisk != null)
                return sameRisk;

            var littleLower = portfolios.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value - 1));
            if (littleLower != null)
                return littleLower;

            var littleHigher = portfolios.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value + 1));
            if (littleHigher != null)
                return littleHigher;

            var lower = portfolios.SingleOrDefault(c => c.Projection.RiskType.Value == (riskType.Value - 2));
            if (lower != null)
                return lower;

            return portfolios.Single(c => c.Projection.RiskType.Value == (riskType.Value + 2));
        }
        
        public DomainObjects.Portfolio.Portfolio GetValidByOwner(int userId, int portfolioId)
        {
            return Data.GetValidByOwner(userId, portfolioId);
        }

        public List<Model.Portfolio> List(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = Task.Factory.StartNew(() => BuyBusiness.ListPurchases(user.Id));
            var portfolios = Data.ListAllValids();
            var portfoliosQty = Task.Factory.StartNew(() => BuyBusiness.ListPortfoliosPurchases(portfolios.Select(c => c.Id)));
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));
            
            Task.WaitAll(purchases, portfoliosQty);
            Task.WaitAll(histories.ToArray());

            portfolios.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());

            return portfolios.Select(c => FillPortfolioModel(c, c.Advisor, user, purchases.Result, portfoliosQty.Result))
                .OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
        }

        public List<Model.Portfolio> ListPurchased(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = BuyBusiness.ListPurchases(user.Id);
            if (purchases.Count == 0)
                return new List<Model.Portfolio>();

            var portfoliosQty = Task.Factory.StartNew(() => BuyBusiness.ListPortfoliosPurchases(purchases.Select(c => c.PortfolioId)));
            var portfolios = Task.Factory.StartNew(() => PortfolioBusiness.ListWithDetails(purchases.Select(c => c.PortfolioId)));
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (var buy in purchases)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(buy.PortfolioId)));

            Task.WaitAll(portfolios, portfoliosQty);
            Task.WaitAll(histories.ToArray());

            portfolios.Result.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());

            return portfolios.Result.Select(c => FillPortfolioModel(c, c.Advisor, user, purchases, portfoliosQty.Result))
                .OrderByDescending(c => c.PurchaseQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
        }
        
        public Model.Portfolio FillPortfolioModel(DomainObjects.Portfolio.Portfolio portfolio, DomainObjects.Advisor.Advisor advisor, User user,
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
                Owned = user != null && advisor.UserId == user.Id,
                Purchased = purchases != null && purchases.Any(x => x.PortfolioId == portfolio.Id),
                Enabled = portfolio.Detail.Enabled,
                PurchaseQuantity = purchasesQty.ContainsKey(portfolio.Id) ? purchasesQty[portfolio.Id] : 0,
                TotalDays = portfolio.PortfolioHistory.Count,
                LastDay = PortfolioHistoryBusiness.GetHistoryResult(1, portfolio.PortfolioHistory),
                Last7Days = PortfolioHistoryBusiness.GetHistoryResult(7, portfolio.PortfolioHistory),
                Last30Days = PortfolioHistoryBusiness.GetHistoryResult(30, portfolio.PortfolioHistory),
                AllDays = PortfolioHistoryBusiness.GetHistoryResult((int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolio.PortfolioHistory.Min(x => x.Date)).TotalDays) + 1, portfolio.PortfolioHistory)
            };
        }

        public List<DomainObjects.Portfolio.Portfolio> List(int advisorId, bool onlyEnabled = true)
        {
            return List(new int[] { advisorId }, onlyEnabled).Single().Value;
        }

        public List<DomainObjects.Portfolio.Portfolio> ListWithHistory(int advisorId, bool onlyEnabled = true)
        {
            var portfolios = List(advisorId, onlyEnabled);
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));
            
            Task.WaitAll(histories.ToArray());

            portfolios.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());
            return portfolios;
        }

        public Dictionary<int, List<DomainObjects.Portfolio.Portfolio>> List(IEnumerable<int> advisorId, bool onlyEnabled = true)
        {
            var portfolio = Data.ListByAdvisor(advisorId, onlyEnabled);
            return portfolio.GroupBy(c => c.AdvisorId, c => c, (k, v) => new KeyValuePair<int, List<DomainObjects.Portfolio.Portfolio>>(k, v.ToList())).ToDictionary(c => c.Key, c => c.Value);
        }

        private List<DomainObjects.Portfolio.Portfolio> FillPortfoliosDistribution(List<DomainObjects.Portfolio.Portfolio> portfolio)
        {
            var distributions = DistributionBusiness.List(portfolio.Select(c => c.ProjectionId.Value));
            portfolio.ForEach(c => c.Projection.Distribution = distributions.Where(d => d.ProjectionId == c.ProjectionId.Value).ToList());
            return portfolio;
        }

        public void UpdateAllPortfoliosHistory()
        {
            var portfolios = Data.ListAll();
            foreach(var portfolio in portfolios)
            {
                PortfolioHistoryBusiness.UpdatePortfolioHistory(portfolio);
            }
        }
    }
}
