using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class Advisor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Period { get; set; }
        public bool Purchased { get; set; }
        public int PurchaseQuantity { get; set; }
        public List<RiskProjection> Projection { get; set; }
        public Details Detail { get; set; }

        public class RiskProjection
        {
            public int Risk { get; set; }
            public double ProjectionPercent { get; set; }
            public double? OptimisticPercent { get; set; }
            public double? PessimisticPercent { get; set; }
        }

        public class Details
        {
            public Purchase PurchaseInfo { get; set; }
            public List<History> PortfolioHistory { get; set; }
        }

        public class History
        {
            public int Risk { get; set; }
            public int TotalDays { get; set; }
            public HistoryResult LastDay { get; set; }
            public HistoryResult Last7Days { get; set; }
            public HistoryResult Last30Days { get; set; }
            public HistoryResult AllDays { get; set; }
            public List<Distribution> Histogram { get; set; }
        }

        public class Distribution
        {
            public double GreaterOrEqual { get; set; }
            public double Lesser { get; set; }
            public int Quantity { get; set; }
        }

        public class HistoryResult
        {
            public double Value { get; set; }
            public double ExpectedValue { get; set; }
            public double? OptimisticExpectation { get; set; }
            public double? PessimisticExpectation { get; set; }
            public double HitPercentage { get; set; }
        }

        public class Purchase
        {
            public int QtyAlreadyPurchased { get; set; }
            public DateTime? ExpirationTime { get; set; }
            public int? Risk { get; set; }
        }
    }
}
