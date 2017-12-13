using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class AdvisorData : BaseData<Advisor>
    {
        public override string TableName => "Advisor";
    }
}
