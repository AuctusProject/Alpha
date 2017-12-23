using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class Projections
    {
        public Goal CurrentGoal { get; set; }
        public List<Purchase> Purchases { get; set; }

        public class Purchase
        {
            public DateTime PurchaseDate { get; set; }
            public DateTime ExpirationDate { get; set; }
            public double Price { get; set; }
            public int Period { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public int AdvisorId { get; set; }
            public Goal Goal { get; set; }
            public Projection ProjectionData { get; set; }
        }

        public class Projection
        {
            public int Risk { get; set; }
            public double ProjectionPercent { get; set; }
            public double? OptimisticPercent { get; set; }
            public double? PessimisticPercent { get; set; }
            public double? PurchasedProjectionValue { get; set; }
            public double? PurchasedOptimisticValue { get; set; }
            public double? PurchasedPessimisticValue { get; set; }
            public double? CurrentProjectionValue { get; set; }
            public double? CurrentOptimisticValue { get; set; }
            public double? CurrentPessimisticValue { get; set; }
        }

        public class Goal
        {
            public double? TargetAmount { get; set; }
            public double? StartingAmount { get; set; }
            public double? MonthlyContribution { get; set; }
            public int? Timeframe { get; set; }
            public int Risk { get; set; }
        }
    }
}
