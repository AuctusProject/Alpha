using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class PortfolioDistribution
    {
        public int AdvisorId { get; set; }
        public List<Asset> Distribution { get; set; }

        public class Asset
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Code { get; set; }
            public int Type { get; set; }
            public double Percentage { get; set; }
        }
    }
}
