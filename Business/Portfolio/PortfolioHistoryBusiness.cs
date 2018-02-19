using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Asset;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Auctus.Business.Portfolio
{
    public class PortfolioHistoryBusiness : BaseBusiness<PortfolioHistory, PortfolioHistoryData>
    {
        public PortfolioHistoryBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

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
                        RealValue = portfolioRealValue - 100.0,
                        ProjectionValue = projection.ProjectionValue,
                        OptimisticProjectionValue = projection.OptimisticProjectionValue,
                        PessimisticProjectionValue = projection.PessimisticProjectionValue
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
        
        public List<PortfolioHistory> ListHistory(int portfolioId)
        {
            string cacheKey = string.Format("PortfolioHistory{0}", portfolioId);
            var portfolioHistory = MemoryCache.Get<List<PortfolioHistory>>(cacheKey);
            if (portfolioHistory == null || !portfolioHistory.Any() || portfolioHistory.Last().Date.Date != DateTime.UtcNow.Date)
            {
                portfolioHistory = Data.ListHistory(portfolioId);
                if (portfolioHistory?.LastOrDefault()?.Date.Date == DateTime.UtcNow.Date)
                    MemoryCache.Set<List<PortfolioHistory>>(cacheKey, portfolioHistory, 720);
            }
            return portfolioHistory;
        }

        public Model.Portfolio.HistoryResult GetHistoryResult(IEnumerable<PortfolioHistory> portfolioHistory)
        {
            int days = (portfolioHistory != null && portfolioHistory.Any()) ? 
                (int)Math.Ceiling(DateTime.UtcNow.Subtract(portfolioHistory.Min(x => x.Date)).TotalDays) + 1 : 1;
            return GetHistoryResult(days, portfolioHistory);
        }

        public Model.Portfolio.HistoryResult GetHistoryResult(int days, IEnumerable<PortfolioHistory> portfolioHistory)
        {
            var history = portfolioHistory.Where(c => c.Date >= DateTime.UtcNow.AddDays(-days).Date);
            return !history.Any() ? null : new Model.Portfolio.HistoryResult()
            {
                Value = (history.Select(c => 1 + (c.RealValue / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                ExpectedValue = (history.Select(c => 1 + (c.ProjectionValue / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                OptimisticExpectation = history.Any(c => !c.OptimisticProjectionValue.HasValue) ? (double?)null :
                                        (history.Select(c => 1 + (c.OptimisticProjectionValue.Value / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                PessimisticExpectation = history.Any(c => !c.PessimisticProjectionValue.HasValue) ? (double?)null :
                                        (history.Select(c => 1 + (c.PessimisticProjectionValue.Value / 100.0)).Aggregate((mult, c) => c * mult) - 1) * 100,
                HitPercentage = (double)history.Count(c => c.RealValue >= c.ProjectionValue) / (double)history.Count() * 100
            };
        }

        public List<Model.Portfolio.HistogramDistribution> GetHistogram(IEnumerable<PortfolioHistory> portfolioHistory)
        {
            if (!portfolioHistory.Any())
                return new List<Model.Portfolio.HistogramDistribution>();

            var values = portfolioHistory.OrderBy(c => c.RealValue).Select(c => c.RealValue);
            var minValue = values.First();
            var maxValue = values.Last();
            var difference = maxValue - minValue;
            double rangeGroup;
            if (difference == 0)
                rangeGroup = 1;
            else
                rangeGroup = difference / (values.Count() > 75 ? 15.0 : 
                    values.Count() > 55 ? 12.0 :
                    values.Count() > 35 ? 9.0 :
                    values.Count() > 15 ? 7.0 :
                    values.Count() > 10 ? 5.0 :
                    values.Count() > 5 ? 4.0 :
                    values.Count() == 1 ? 1.0 : values.Count() - 1);

            minValue = minValue - (rangeGroup / 1.5);
            maxValue = maxValue + (rangeGroup / 1.5);

            List<Model.Portfolio.HistogramDistribution> result = new List<Model.Portfolio.HistogramDistribution>();
            for (double i = minValue; i <= maxValue; i = i + rangeGroup)
            {
                result.Add(new Model.Portfolio.HistogramDistribution()
                {
                    GreaterOrEqual = i,
                    Lesser = i + rangeGroup,
                    Quantity = portfolioHistory.Count(c => c.RealValue >= i && c.RealValue < (i + rangeGroup))
                });
            }
            return result;
        }
    }
}
