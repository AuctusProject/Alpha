using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class Investments
    {
        public List<Portfolio> PurchasedPortfolios { get; set; }
        public List<ExchangePortfolio> ExchangePortfolios { get; set; }

        public class ExchangePortfolio
        {
            public int ExchangeId { get; set; }
            public string Name { get; set; }
        }
    }
}
