using Auctus.DomainObjects.Portfolio;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Portfolio
{
    public class PortfolioDetailData : BaseData<PortfolioDetail>
    {
        public override string TableName => "PortfolioDetail";
    }
}
