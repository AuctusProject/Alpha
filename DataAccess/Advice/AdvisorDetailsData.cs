using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class AdvisorDetailsData : BaseData<AdvisorDetails>
    {
        public override string TableName => "AdvisorDetails";
    }
}
