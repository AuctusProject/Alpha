using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class BuyData : BaseData<Buy>
    {
        public override string TableName => "Buy";
    }
}
