using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class DistributionData : BaseData<Distribution>
    {
        public override string TableName => "Distribution";
    }
}
