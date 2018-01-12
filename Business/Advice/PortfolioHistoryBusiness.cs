using Auctus.DataAccess.Advice;
using Auctus.DomainObjects.Advice;
using Auctus.DomainObjects.Asset;
using Auctus.Model;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.Business.Advice
{
    public class PortfolioHistoryBusiness : BaseBusiness<DomainObjects.Advice.PortfolioHistory, PortfolioHistoryData>
    {
        public PortfolioHistoryBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public void UpdatePortfolioHistory(Portfolio portfolio)
        {
            var lastUpdatedValue = LastPortfolioHistoryDate(portfolio.Id);
            if (lastUpdatedValue >= DateTime.Now.Date)
            {
                return;
            }

            var assetsIds = portfolio.Projections.SelectMany(p => p.Distribution.Select(d => d.AssetId)).Distinct();
            var assetValuesByDate = AssetValueBusiness.GetAssetValuesGroupedByDate(assetsIds, lastUpdatedValue ?? portfolio.CreationDate).OrderBy(v => v.Key).ToList();
            CreatePortfolioHistoryForEachAssetValueDate(portfolio, assetValuesByDate);
        }

        private void CreatePortfolioHistoryForEachAssetValueDate(Portfolio portfolio, IList<KeyValuePair<DateTime, List<AssetValue>>> assetValuesByDate)
        {
            if (assetValuesByDate.Any())
            {
                KeyValuePair<DateTime, List<AssetValue>> previousAssetValue = GetFirstAssetValueAndRemoveFromList(assetValuesByDate);
                foreach (var assetValue in assetValuesByDate)
                {
                    var portfolioHistory = CreatePortfolioHistoryForDate(assetValue.Key, portfolio, assetValue.Value, previousAssetValue.Value);
                    if (portfolioHistory != null)
                    {
                        previousAssetValue = assetValue;
                    }
                }
            }
        }

        private static KeyValuePair<DateTime, List<AssetValue>> GetFirstAssetValueAndRemoveFromList(IList<KeyValuePair<DateTime, List<AssetValue>>> assetValuesByDate)
        {
            var previousAssetValue = assetValuesByDate.FirstOrDefault();
            assetValuesByDate.Remove(previousAssetValue);
            return previousAssetValue;
        }

        private DomainObjects.Advice.PortfolioHistory CreatePortfolioHistoryForDate(DateTime date, Portfolio portfolio, List<AssetValue> currentAssetValues, List<AssetValue> previousAssetValues)
        {
            var projection = PortfolioBusiness.GetProjectionAtDate(date, portfolio);
            if (projection != null)
            {
                var assetsIds = projection.Distribution.Select(d => d.AssetId).Distinct();
                if (assetsIds.Count() == currentAssetValues.Count && assetsIds.Count() == previousAssetValues.Count)
                {
                    var portfolioRealValue = 0.0;
                    foreach (var assetDistribution in projection.Distribution)
                    {
                        var currentAssetValue = currentAssetValues.FirstOrDefault(a => a.AssetId == assetDistribution.AssetId);
                        var previousAssetValue = previousAssetValues.FirstOrDefault(a => a.AssetId == assetDistribution.AssetId);

                        portfolioRealValue += currentAssetValue.Value / previousAssetValue.Value * assetDistribution.Percent;
                    }
                    var portfolioHistory = new DomainObjects.Advice.PortfolioHistory()
                    {
                        Date = date,
                        PortfolioId = portfolio.Id,
                        RealValue = portfolioRealValue,
                        ProjectionValue = projection.ProjectionValue,
                        OptimisticProjectionValue = projection.OptimisticProjection,
                        PessimisticProjectionValue = projection.PessimisticProjection
                    };
                    Data.Insert(portfolioHistory);
                    return portfolioHistory;
                }
            }
            return null;
        }

        private DateTime? LastPortfolioHistoryDate(int id)
        {
            var portfolioHistory = Data.LastPortfolioHistory(id);
            return portfolioHistory?.Date;
        }

        public List<Model.PortfolioHistory> ListPortfolioHistory(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = BuyBusiness.ListPurchasesWithPortfolio(user.Id);
            List<Model.PortfolioHistory> result = new List<Model.PortfolioHistory>();
            foreach (Buy buy in purchases)
            {
                Model.PortfolioHistory portfolioHistory = new Model.PortfolioHistory();
                portfolioHistory.AdvisorId = buy.AdvisorId;
                portfolioHistory.Values = ListPortfolioHistory(buy.Projection.PortfolioId).Select(c => new Model.PortfolioHistory.HistoryValue()
                {
                    Date = c.Date,
                    Value = c.RealValue
                }).ToList();
                result.Add(portfolioHistory);
            }
            return result;
        }

        private List<DomainObjects.Advice.PortfolioHistory> ListPortfolioHistory(int portfolioId)
        {
            string cacheKey = string.Format("PortfolioHistory{0}", portfolioId);
            var portfolioHistory = MemoryCache.Get<List<DomainObjects.Advice.PortfolioHistory>>(cacheKey);
            if (portfolioHistory == null || !portfolioHistory.Any() || portfolioHistory.Last().Date.Date != DateTime.UtcNow.Date)
            {
                portfolioHistory = Data.ListPortfolioHistory(portfolioId);
                if (portfolioHistory == null)
                    throw new ArgumentException("Portfolio history cannot be found.");
                else if (portfolioHistory.Last().Date.Date == DateTime.UtcNow.Date)
                    MemoryCache.Set<List<DomainObjects.Advice.PortfolioHistory>>(cacheKey, portfolioHistory, 720);
            }
            return portfolioHistory;
        }
    }
}
