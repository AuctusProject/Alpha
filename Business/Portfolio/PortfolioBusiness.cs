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
using static Auctus.Model.Investments;

namespace Auctus.Business.Portfolio
{
    public class PortfolioBusiness : BaseBusiness<DomainObjects.Portfolio.Portfolio, PortfolioData>
    {
        public PortfolioBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public DomainObjects.Portfolio.Portfolio Create(string email, int advisorId, decimal price, string name, string description, 
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

        public DomainObjects.Portfolio.Portfolio Update(string email, int portfolioId, decimal price, string name, string description)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            portfolio.Detail = PortfolioDetailBusiness.Create(portfolioId, price, name, description, true);
            return portfolio;
        }

        public DomainObjects.Portfolio.Portfolio UpdateWithDistribution(string email, int portfolioId, decimal price, string name, 
            string description, Dictionary<int, double> distribution)
        {
            var user = UserBusiness.GetValidUser(email);
            var portfolio = GetValidByOwner(user.Id, portfolioId);
            if (portfolio == null)
                throw new ArgumentException("Invalid portfolio.");

            var projection = ProjectionBusiness.Get(portfolio.ProjectionId.Value);
            if (projection == null)
                throw new ArgumentException("Invalid projection.");

            var risk = PortfolioBusiness.GetRisk(projection.ProjectionValue, distribution);
            using (var transaction = new TransactionalDapperCommand())
            {
                var portfolioDetail = PortfolioDetailBusiness.SetNew(portfolioId, price, name, description, true);
                transaction.Insert(portfolioDetail);

                var newProjection = ProjectionBusiness.SetNew(portfolio.Id, projection.ProjectionValue, risk, projection.OptimisticProjectionValue, projection.PessimisticProjectionValue);
                transaction.Insert(newProjection);

                var distributions = DistributionBusiness.SetNew(newProjection.Id, distribution);
                foreach (Distribution dist in distributions)
                    transaction.Insert(dist);

                portfolio.ProjectionId = newProjection.Id;
                transaction.Update(portfolio);

                newProjection.Distribution = distributions;
                portfolio.Detail = portfolioDetail;
                portfolio.Projection = newProjection;
                transaction.Commit();
            }
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
            double cryptoAssetsPercentage = assets.Count(c => c.Type == AssetType.Crypto.Value) / assets.Count() * 100.0;
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

        public DomainObjects.Portfolio.Portfolio GetSimple(int portfolioId)
        {
            return Data.GetSimple(portfolioId);
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
                portfolios = List(roboAdvisorId).Where(c => c.Advisor.Type == DomainObjects.Advisor.AdvisorType.Robo.Value).ToList();
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
            var following = Task.Factory.StartNew(() => FollowBusiness.ListFollowing(user.Id));
            var portfolios = Data.ListAllValids();
            var portfoliosQty = Task.Factory.StartNew(() => FollowBusiness.ListPortfoliosFollowers(portfolios.Select(c => c.Id)));
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (DomainObjects.Portfolio.Portfolio portfolio in portfolios)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolio.Id)));
            
            Task.WaitAll(following, portfoliosQty);
            Task.WaitAll(histories.ToArray());

            portfolios.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());

            return portfolios.Select(c => FillPortfolioModel(c, c.Advisor, user, following.Result, portfoliosQty.Result))
                .OrderByDescending(c => c.FollowersQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
        }

        public List<Model.Investments.ExchangePortfolio> ListExchangePortfolios(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var exchangeApis = ExchangeApiAccessBusiness.List(email);
            var result = new List<Model.Investments.ExchangePortfolio>();
            foreach(var exchangeApi in exchangeApis)
            {
                result.Add(
                    new Model.Investments.ExchangePortfolio()
                    {
                        ExchangeId = exchangeApi.ExchangeId,
                        Name = $"My {Exchange.GetName(exchangeApi.ExchangeId)} portfolio"
                    }
                );
            }
            return result;
        }

        public Model.Investments GetInvestments(string email)
        {
            var investments = new Model.Investments();
            investments.PurchasedPortfolios = ListFollowing(email);
            investments.ExchangePortfolios = Enumerable.Empty<ExchangePortfolio>().ToList(); //ListExchangePortfolios(email);
            return investments;
        }

        public List<Model.Portfolio> ListFollowing(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var following = FollowBusiness.ListFollowing(user.Id);
            if (following.Count == 0)
                return new List<Model.Portfolio>();

            var portfoliosQty = Task.Factory.StartNew(() => FollowBusiness.ListPortfoliosFollowers(following.Select(c => c.PortfolioId)));
            var portfolios = Task.Factory.StartNew(() => Data.List(following.Select(c => c.PortfolioId)));
            List<Task<List<PortfolioHistory>>> histories = new List<Task<List<PortfolioHistory>>>();
            foreach (var follow in following)
                histories.Add(Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(follow.PortfolioId)));

            Task.WaitAll(portfolios, portfoliosQty);
            Task.WaitAll(histories.ToArray());

            portfolios.Result.ForEach(c => c.PortfolioHistory = histories.SelectMany(x => x.Result.Where(g => g.PortfolioId == c.Id)).ToList());

            return portfolios.Result.Select(c => FillPortfolioModel(c, c.Advisor, user, following, portfoliosQty.Result))
                .OrderByDescending(c => c.FollowersQuantity).ThenByDescending(c => c.ProjectionPercent).ToList();
        }

        public DomainObjects.Portfolio.Portfolio Get(int id)
        {
            return Data.List(new int[] { id }).SingleOrDefault();
        }

        public Model.Portfolio Get(string email, int portfolioId)
        {
            var user = UserBusiness.GetValidUser(email);
            var follow = Task.Factory.StartNew(() => FollowBusiness.Get(user.Id, portfolioId));
            var portfolio = Task.Factory.StartNew(() => Get(portfolioId));
            Task.WaitAll(portfolio, follow);

            var owned = user.Id == portfolio.Result.Advisor.UserId;
            var followed = follow.Result != null;

            if (portfolio.Result == null || (!owned && !followed && (!portfolio.Result.Detail.Enabled || !portfolio.Result.Advisor.Detail.Enabled)))
                throw new ArgumentException("Invalid portfolio.");
            
            var portfolioQty = Task.Factory.StartNew(() => FollowBusiness.ListPortfoliosFollowers(new int[] { portfolioId }));
            var history = Task.Factory.StartNew(() => PortfolioHistoryBusiness.ListHistory(portfolioId));
            var distribution = Task.Factory.StartNew(() => DistributionBusiness.List(new int[] { portfolio.Result.ProjectionId.Value }));
            
            if (owned)
            {
                Task<List<EscrowResult>> escrowResult = Task.Factory.StartNew(() => EscrowResultBusiness.ListByPortfolio(portfolio.Result.Id));
                Task.WaitAll(history, portfolioQty, distribution, escrowResult);
            }
            else
            {
                Task.WaitAll(history, portfolioQty, distribution);
            }

            portfolio.Result.PortfolioHistory = history.Result;
            var result = FillPortfolioModel(portfolio.Result, portfolio.Result.Advisor, user, 
                follow.Result != null ? new Follow[] { follow.Result } : null, portfolioQty.Result);

            result.Following = followed;
            result.Histogram = PortfolioHistoryBusiness.GetHistogram(history.Result);
            result.HistoryData = history.Result.Select(c => new Model.Portfolio.History()
            {
                Date = c.Date,
                Value = c.RealValue
            }).ToList();
            result.AssetDistribution = distribution == null ? null : distribution.Result.Select(c => new Model.Portfolio.Distribution()
            {
                Id = c.AssetId,
                Code = c.Asset.Code,
                Name = c.Asset.Name,
                Percentage = c.Percent,
                Type = (int)c.Asset.Type
            }).OrderByDescending(c => c.Percentage).ToList();
            return result;
        }

        public Model.Portfolio FillPortfolioModel(DomainObjects.Portfolio.Portfolio portfolio, DomainObjects.Advisor.Advisor advisor, User user,
            IEnumerable<Follow> follows, Dictionary<int, int> followersQty)
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
                AdvisorType = (int)advisor.Type,
                Risk = portfolio.Projection.Risk,
                ProjectionPercent = portfolio.Projection.ProjectionValue,
                OptimisticPercent = portfolio.Projection.OptimisticProjectionValue,
                PessimisticPercent = portfolio.Projection.PessimisticProjectionValue,
                Owned = user != null && advisor.UserId == user.Id,
                Following = follows != null && follows.Any(x => x.PortfolioId == portfolio.Id),
                Enabled = portfolio.Detail.Enabled && advisor.Detail.Enabled,
                FollowersQuantity = followersQty.ContainsKey(portfolio.Id) ? followersQty[portfolio.Id] : 0,
                TotalDays = portfolio.PortfolioHistory.Count,
                LastDay = PortfolioHistoryBusiness.GetHistoryResult(1, portfolio.PortfolioHistory),
                Last7Days = PortfolioHistoryBusiness.GetHistoryResult(7, portfolio.PortfolioHistory),
                Last30Days = PortfolioHistoryBusiness.GetHistoryResult(30, portfolio.PortfolioHistory),
                AllDays = PortfolioHistoryBusiness.GetHistoryResult(portfolio.PortfolioHistory)
            };
        }

        public List<DomainObjects.Portfolio.Portfolio> List(int advisorId, bool onlyEnabled = true)
        {
            var advisorData = List(new int[] { advisorId }, onlyEnabled).SingleOrDefault();
            return advisorData.Equals(default(KeyValuePair<int, List<DomainObjects.Portfolio.Portfolio>>))
                ? new List<DomainObjects.Portfolio.Portfolio>() : advisorData.Value;
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

        public Model.ExchangePortfolio GetExchangePortfolio(string email, int exchangeId)
        {
            var user = UserBusiness.GetValidUser(email);
            var exchangeApiAccess = ExchangeApiAccessBusiness.Get(user.Id, exchangeId);

            var exchangeApi = DataAccess.Exchanges.ExchangeApi.GetById(exchangeApiAccess.ExchangeId, exchangeApiAccess.ApiKey, exchangeApiAccess.ApiSecretKey);
            var exchangeBalances = exchangeApi.GetBalances();
            var assetDistribution = ExchangeApiAccessBusiness.ConvertExchangeBalancesToAssetDistribution(exchangeBalances);

            return new Model.ExchangePortfolio()
            {
                ExchangeId = exchangeId,
                Name = $"My {Exchange.GetName(exchangeId)} portfolio",                
                AssetDistribution = assetDistribution
            };
        }

        public void DeleteExchangePortfolio(string email, int exchangeId)
        {
            var user = UserBusiness.GetValidUser(email);
            var exchangeApiAccess = ExchangeApiAccessBusiness.Get(user.Id, exchangeId);
            ExchangeApiAccessBusiness.Delete(exchangeApiAccess);
        }
    }
}

