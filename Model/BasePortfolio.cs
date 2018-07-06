using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class BasePortfolio
    {
        public string Name { get; set; }
        public List<HistogramDistribution> Histogram { get; set; }
        public List<History> HistoryData { get; set; }
        public List<Distribution> AssetDistribution { get; set; }
        public List<DistributionHistory> AssetDistributionHistory { get; set; }

        public class DistributionHistory
        {
            public DateTime Date { get; set; }
            public List<Distribution> AssetDistribution { get; set; }
        }

        public class Distribution
        {
            public int Id { get; set; }
            public string Code { get; set; }
            public string Name { get; set; }
            public int Type { get; set; }
            public double Percentage { get; set; }
            public double? CurrentPercentage { get; set; }
        }
        
        public class HistogramDistribution
        {
            public double GreaterOrEqual { get; set; }
            public double Lesser { get; set; }
            public int Quantity { get; set; }
        }

        public class History
        {
            public DateTime Date { get; set; }
            public double Value { get; set; }
        }
    }
}
