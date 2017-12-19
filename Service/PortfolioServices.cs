using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class PortfolioServices : BaseServices
    {
        public PortfolioServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public void UpdatePortfoliosHistory()
        {
            PortfolioBusiness.UpdateAllPortfoliosHistory();
        }
    }
}
