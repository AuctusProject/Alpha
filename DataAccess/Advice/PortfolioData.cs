using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class PortfolioData : BaseData<Portfolio>
    {
        public override string TableName => "Portfolio";
    }
}
