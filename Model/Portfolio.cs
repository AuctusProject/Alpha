using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class Portfolio : BasePortfolio
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Purchased { get; set; }
        public bool Owned { get; set; }
        public bool Enabled { get; set; }
        public int PurchaseQuantity { get; set; }
        public int AdvisorId { get; set; }
        public string AdvisorName { get; set; }
        public string AdvisorDescription { get; set; }
        public int AdvisorType { get; set; }
        public int Risk { get; set; }
        public double ProjectionPercent { get; set; }
        public double? OptimisticPercent { get; set; }
        public double? PessimisticPercent { get; set; }
        public int TotalDays { get; set; }
        public HistoryResult LastDay { get; set; }
        public HistoryResult Last7Days { get; set; }
        public HistoryResult Last30Days { get; set; }
        public HistoryResult AllDays { get; set; }
        public Purchase PurchaseData { get; set; }
        public Owner OwnerData { get; set; }
        public int? BuyTransactionStatus { get; set; }
        public string BuyTransactionHash { get; set; }
        public int? BuyTransactionId { get; set; }
        public class Owner
        {
            public decimal AucEscrow { get; set; }
            public decimal AucReached { get; set; }
            public decimal AucLost { get; set; }
        }

        public class Purchase
        {
            public decimal Price { get; set; }
            public decimal AucEscrow { get; set; }
            public int Risk { get; set; }
            public DateTime CreationDate { get; set; }
            public DateTime? ExpirationDate { get; set; }
            public int TransactionStatus { get; set; }
            public Goal Goal { get; set; }
        }

        public class Goal
        {
            public double? TargetAmount { get; set; }
            public double StartingAmount { get; set; }
            public double MonthlyContribution { get; set; }
            public int Timeframe { get; set; }
            public int Risk { get; set; }
        }

        
        public class HistoryResult
        {
            public double Value { get; set; }
            public double ExpectedValue { get; set; }
            public double? OptimisticExpectation { get; set; }
            public double? PessimisticExpectation { get; set; }
            public double HitPercentage { get; set; }
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
    }
}
