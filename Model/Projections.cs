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
            public Result Current { get; set; }
            public Result Purchased { get; set; }
        }
        
        public class Result
        {
            public double? ProjectionValue { get; set; }
            public double? OptimisticValue { get; set; }
            public double? PessimisticValue { get; set; }
            public bool? Reached { get; set; }
            public double? Difference { get; set; }
            public double? NewStartingAmount { get; set; }
            public double? NewMonthlyContribution { get; set; }
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
