using Auctus.DataAccess.Exchanges;
using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Asset;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auctus.Business.Portfolio
{
    public class PortfolioHistoryBusiness : BaseBusiness<PortfolioHistory, PortfolioHistoryData>
    {
        public PortfolioHistoryBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        //public void UpdatePortfolioHistory(DomainObjects.Portfolio.Portfolio portfolio)
        //{
        //    var lastUpdatedValue = LastPortfolioHistoryDate(portfolio.Id);
        //    if (lastUpdatedValue?.AddMinutes(DataAccess.Exchanges.ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES) > DateTime.UtcNow)
        //    {
        //        return;
        //    }
        //    var assetsIds = portfolio.Projections.SelectMany(p => p.Distribution.Select(d => d.AssetId)).Distinct();
        //    var assetValuesByDate = AssetValueBusiness.GetAssetValuesGroupedByDate(assetsIds, lastUpdatedValue ?? portfolio.CreationDate).OrderBy(v => v.Key).ToList();
        //    CreatePortfolioHistoryForEachAssetValueDate(portfolio, assetValuesByDate);
        //}

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
                var distributionAssetsIds = projection.Distribution.Select(d => d.AssetId).Distinct();
                var currentAssetsIds = currentAssetValues.Select(c => c.AssetId).Distinct();
                var previousAssetsIds = previousAssetValues.Select(p => p.AssetId).Distinct();
                if (IsAssetListMatch(distributionAssetsIds, currentAssetsIds) && IsAssetListMatch(currentAssetsIds, previousAssetsIds))
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
                        RealValue = portfolioRealValue - 100.0
                    };
                    Data.InsertOneAsync(portfolioHistory);
                    return portfolioHistory;
                }
            }
            return null;
        }

        internal bool IsAssetListMatch(IEnumerable<int> list1, IEnumerable<int> list2)
        {
            if (list1?.Count() == list2?.Count())
            {
                var firstNotSecond = list1.Except(list2);
                var secondNotFirst = list2.Except(list1);
                return !firstNotSecond.Any() && !secondNotFirst.Any();
            }
            return false;
        }

        //private DateTime? LastPortfolioHistoryDate(int id)
        //{
        //    var portfolioHistory = Data.LastHistory(id);
        //    return portfolioHistory?.Date;
        //}
        
        private List<PortfolioHistory> GetByCache(int portfolioId)
        {
            var portfolioHistory = MemoryCache.Get<List<PortfolioHistory>>(GetCacheKey(portfolioId));
            if (portfolioHistory == null || !portfolioHistory.Any())
                return null;
            else
                return portfolioHistory;
        }

        private string GetCacheKey(int portfolioId)
        {
            return string.Format("PortfolioHistory{0}", portfolioId);
        }

        private List<PortfolioHistory> GetPortfolioHistoryDataSettingCache(int portfolioId, IOrderedEnumerable<KeyValuePair<DateTime, List<AssetValue>>> assetValues, 
            IEnumerable<Distribution> distributions)
        {
            var portfolioHistory = GetPortfolioHistoryData(assetValues, PortfolioBusiness.ConvertDistributionToModel(distributions)).Select(c => new PortfolioHistory()
            {
                Date = c.Date,
                RealValue = c.Value,
                PortfolioId = portfolioId
            }).ToList();
            if (portfolioHistory.Count > 0 && portfolioHistory.Max(c => c.Date).AddMinutes(ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES * 1.5) > DateTime.UtcNow)
                MemoryCache.Set<List<PortfolioHistory>>(GetCacheKey(portfolioId), portfolioHistory, ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES);
            return portfolioHistory;
        }

        public List<PortfolioHistory> ListHistory(Dictionary<int, DateTime> portfolioData, IEnumerable<Distribution> distributions = null,
            IOrderedEnumerable<KeyValuePair<DateTime, List<AssetValue>>> assetValues = null)
        {
            var result = new List<PortfolioHistory>();
            var pendingPortfolios = new Dictionary<int, DateTime>();
            foreach (var data in portfolioData)
            {
                var portfolioHistory = GetByCache(data.Key);
                if (portfolioHistory != null)
                    result.AddRange(portfolioHistory);
                else
                    pendingPortfolios[data.Key] = data.Value;
            }
            if (pendingPortfolios.Count > 0)
            {
                distributions = distributions ?? DistributionBusiness.ListFromPortfolioId(pendingPortfolios.Select(c => c.Key));
                var portfoliosIds = distributions.Select(c => c.PortfolioId).Distinct();
                pendingPortfolios = pendingPortfolios.Where(c => portfoliosIds.Contains(c.Key)).ToDictionary(c => c.Key, c => c.Value);
                if (pendingPortfolios.Count > 0)
                {
                    var minDate = pendingPortfolios.Min(c => c.Value);
                    assetValues = assetValues ?? AssetValueBusiness.GetAssetValuesGroupedByDate(distributions.Select(c => c.AssetId).Distinct(), minDate).OrderBy(c => c.Key);
                    foreach (var portfolio in pendingPortfolios)
                    {
                        var portfolioHistory = GetPortfolioHistoryDataSettingCache(portfolio.Key,
                            assetValues.Where(c => c.Key > portfolio.Value).OrderBy(c => c.Key),
                            distributions.Where(c => c.PortfolioId == portfolio.Key));
                        if (portfolioHistory?.Count > 0)
                            result.AddRange(portfolioHistory);
                    }
                }
            }
            return result;
        }

        //public List<PortfolioHistory> ListHistory(int portfolioId, DateTime? creationDate = null, IEnumerable<Distribution> distributions = null)
        //{
        //    var portfolioHistory = GetByCache(portfolioId);
        //    if (portfolioHistory != null)
        //        return portfolioHistory;

        //    Task<DomainObjects.Portfolio.Portfolio> portfolio = null;
        //    Task<List<Distribution>> distributionData = null;
        //    if (!creationDate.HasValue)
        //        portfolio = Task.Factory.StartNew(() => PortfolioBusiness.GetSimple(portfolioId));
        //    if (distributions == null)
        //        distributionData = Task.Factory.StartNew(() => DistributionBusiness.ListFromPortfolioId(portfolioId));

        //    if (portfolio != null && distributionData != null)
        //    {
        //        Task.WaitAll(portfolio, distributionData);
        //        creationDate = portfolio.Result.CreationDate;
        //        distributions = distributionData.Result;
        //    }
        //    else if (portfolio != null)
        //    {
        //        Task.WaitAll(portfolio);
        //        creationDate = portfolio.Result.CreationDate;
        //    }
        //    else if (distributionData != null)
        //    {
        //        Task.WaitAll(distributionData);
        //        distributions = distributionData.Result;
        //    }

        //    var assetValues = AssetValueBusiness.GetAssetValuesGroupedByDate(distributions.Select(c => c.AssetId).Distinct(), creationDate.Value).OrderBy(c => c.Key);
        //    portfolioHistory = GetPortfolioHistoryData(assetValues, PortfolioBusiness.ConvertDistributionToModel(distributions)).Select(c => new PortfolioHistory()
        //    {
        //        Date = c.Date,
        //        RealValue = c.Value,
        //        PortfolioId = portfolioId
        //    }).ToList();
        //    if (portfolioHistory.Max(c => c.Date).AddMinutes(ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES * 1.5) > DateTime.UtcNow)
        //        MemoryCache.Set<List<PortfolioHistory>>(GetCacheKey(portfolioId), portfolioHistory, ExchangeApi.GAP_IN_MINUTES_BETWEEN_VALUES);

        //    return portfolioHistory;
        //}

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
            };
        }

        internal List<Model.Portfolio.HistogramDistribution> GetHistogram(IOrderedEnumerable<KeyValuePair<DateTime, List<AssetValue>>> assetValues,
            List<Model.BasePortfolio.DistributionHistory> distribution)
        {
            var portfolioHistory = new List<double>();
            var setted = new HashSet<DateTime>();
            for (int i = 0; i < assetValues.Count(); ++i)
            {
                var baseDate = assetValues.ElementAt(i).Key.Date;
                if (!setted.Contains(baseDate))
                {
                    setted.Add(baseDate);
                    var distributionAtDate = distribution.Where(c => c.Date < assetValues.ElementAt(i).Key).OrderByDescending(c => c.Date).FirstOrDefault();
                    if (distributionAtDate != null)
                    {
                        var previous = assetValues.Where(c => c.Key <= baseDate.AddDays(-1)).OrderByDescending(c => c.Key).FirstOrDefault();
                        var current = assetValues.Where(c => c.Key <= baseDate).OrderByDescending(c => c.Key).FirstOrDefault();
                        var history = GetPortfolioHistory(distributionAtDate, current, previous);
                        if (history != null)
                            portfolioHistory.Add(history.Value);
                    }
                }
            }
            List<Model.Portfolio.HistogramDistribution> result = new List<Model.Portfolio.HistogramDistribution>();
            if (portfolioHistory.Count > 0)
            {
                var values = portfolioHistory.OrderBy(c => c);
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

                for (double i = minValue; i <= maxValue; i = i + rangeGroup)
                {
                    result.Add(new Model.Portfolio.HistogramDistribution()
                    {
                        GreaterOrEqual = i,
                        Lesser = i + rangeGroup,
                        Quantity = portfolioHistory.Count(c => c >= i && c < (i + rangeGroup))
                    });
                }
            }
            return result;
        }

        internal List<Model.Portfolio.History> GetPortfolioHistoryData(IOrderedEnumerable<KeyValuePair<DateTime, List<AssetValue>>> assetValues, 
            List<Model.BasePortfolio.DistributionHistory> distribution)
        {
            var result = new List<Model.BasePortfolio.History>();
            for (int i = 1; i < assetValues.Count(); ++i)
            {
                var distributionAtDate = distribution.Where(c => c.Date < assetValues.ElementAt(i).Key).OrderByDescending(c => c.Date).FirstOrDefault();
                if (distributionAtDate != null)
                {
                    var previous = assetValues.ElementAt(i - 1);
                    var current = assetValues.ElementAt(i);
                    var history = GetPortfolioHistory(distributionAtDate, current, previous);
                    if (history != null)
                        result.Add(history);
                }
            }
            return result;
        }

        private Model.Portfolio.History GetPortfolioHistory(Model.BasePortfolio.DistributionHistory distribution, KeyValuePair<DateTime, List<AssetValue>> current,
            KeyValuePair<DateTime, List<AssetValue>> previous)
        {
            var distributionAssetsIds = distribution.AssetDistribution.Select(c => c.Id).Distinct();
            var currentAssetsIds = current.Value?.Where(c => distributionAssetsIds.Contains(c.AssetId)).Select(c => c.AssetId).Distinct();
            var previousAssetsIds = previous.Value?.Where(c => distributionAssetsIds.Contains(c.AssetId)).Select(c => c.AssetId).Distinct();
            if (PortfolioHistoryBusiness.IsAssetListMatch(distributionAssetsIds, currentAssetsIds) &&
                PortfolioHistoryBusiness.IsAssetListMatch(currentAssetsIds, previousAssetsIds))
            {
                var portfolioRealValue = 0.0;
                foreach (var assetDistribution in distribution.AssetDistribution)
                {
                    var currentAssetValue = current.Value.First(a => a.AssetId == assetDistribution.Id);
                    var previousAssetValue = previous.Value.First(a => a.AssetId == assetDistribution.Id);

                    portfolioRealValue += currentAssetValue.Value / previousAssetValue.Value * assetDistribution.Percentage;
                }
                return new Model.Portfolio.History()
                {
                    Date = current.Key,
                    Value = portfolioRealValue - 100.0
                };
            }
            return null;
        }
    }
}
