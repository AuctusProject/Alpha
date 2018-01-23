using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Asset;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.Business.Portfolio
{
    public class PortfolioHistoryBusiness : BaseBusiness<PortfolioHistory, PortfolioHistoryData>
    {
        public PortfolioHistoryBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public void UpdatePortfolioHistory(DomainObjects.Portfolio.Portfolio portfolio)
        {
            var lastUpdatedValue = LastPortfolioHistoryDate(portfolio.Id);
            if (lastUpdatedValue >= DateTime.UtcNow.Date)
            {
                return;
            }

            var assetsIds = portfolio.Projections.SelectMany(p => p.Distribution.Select(d => d.AssetId)).Distinct();
            var assetValuesByDate = AssetValueBusiness.GetAssetValuesGroupedByDate(assetsIds, lastUpdatedValue ?? portfolio.CreationDate).OrderBy(v => v.Key).ToList();
            CreatePortfolioHistoryForEachAssetValueDate(portfolio, assetValuesByDate);
        }

        private void CreatePortfolioHistoryForEachAssetValueDate(DomainObjects.Portfolio.Portfolio portfolio, IList<KeyValuePair<DateTime, List<AssetValue>>> assetValuesByDate)
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

        private PortfolioHistory CreatePortfolioHistoryForDate(DateTime date, DomainObjects.Portfolio.Portfolio portfolio, List<AssetValue> currentAssetValues, List<AssetValue> previousAssetValues)
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
                    var portfolioHistory = new PortfolioHistory()
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
            var portfolioHistory = Data.LastHistory(id);
            return portfolioHistory?.Date;
        }

        public List<Model.PortfolioHistory> ListHistory(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var purchases = BuyBusiness.ListPurchasesWithPortfolio(user.Id);
            purchases.ForEach(c => c.Projection.Portfolio.PortfolioHistory = ListHistory(c.Projection.PortfolioId));
            List<Model.PortfolioHistory> result = new List<Model.PortfolioHistory>();
            result.AddRange(purchases.Select(c => new Model.PortfolioHistory()
            {
                AdvisorId = c.AdvisorId,
                Values = c.Projection.Portfolio.PortfolioHistory.Select(g => new Model.PortfolioHistory.HistoryValue()
                {
                    Date = g.Date,
                    Value = g.RealValue
                }).ToList(),
                History = AdvisorBusiness.GetPortfolioHistory(new DomainObjects.Portfolio.Portfolio[] { c.Projection.Portfolio })
            }));
            return result;
        }

        public List<PortfolioHistory> ListHistory(int portfolioId)
        {
            string cacheKey = string.Format("PortfolioHistory{0}", portfolioId);
            var portfolioHistory = MemoryCache.Get<List<PortfolioHistory>>(cacheKey);
            if (portfolioHistory == null || !portfolioHistory.Any() || portfolioHistory.Last().Date.Date != DateTime.UtcNow.Date)
            {
                portfolioHistory = Data.ListHistory(portfolioId);
                if (portfolioHistory == null)
                    throw new ArgumentException("Portfolio history cannot be found.");
                else if (portfolioHistory.Last().Date.Date == DateTime.UtcNow.Date)
                    MemoryCache.Set<List<PortfolioHistory>>(cacheKey, portfolioHistory, 720);
            }
            return portfolioHistory;
        }
    }
}
