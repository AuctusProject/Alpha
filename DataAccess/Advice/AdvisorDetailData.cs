using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class AdvisorDetailData : BaseData<AdvisorDetail>
    {
        public override string TableName => "AdvisorDetail";
    }
}
