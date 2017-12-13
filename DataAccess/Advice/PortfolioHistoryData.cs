using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class PortfolioHistoryData : BaseData<PortfolioHistory>
    {
        public override string TableName => "PortfolioHistory";
    }
}
