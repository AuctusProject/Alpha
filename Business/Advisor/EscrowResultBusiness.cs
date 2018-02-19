using Auctus.DataAccess.Advisor;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Advisor
{
    public class EscrowResultBusiness : BaseBusiness<EscrowResult, EscrowResultData>
    {
        public EscrowResultBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public List<EscrowResult> ListByPortfolio(int portfolioId)
        {
            return Data.ListByPortfolio(portfolioId);
        }
    }
}
