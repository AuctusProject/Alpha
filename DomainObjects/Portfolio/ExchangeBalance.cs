using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Portfolio
{
    public class ExchangeBalance
    {
        public string CurrencyCode { get; set; }
        public double Amount { get; set; }

        public Asset.Asset Asset { get; set; }
        public double? CurrentUsdValue { get; set; }
    }
}
