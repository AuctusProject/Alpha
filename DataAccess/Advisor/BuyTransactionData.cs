using Auctus.DomainObjects.Advisor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class BuyTransactionData : BaseData<BuyTransaction>
    {
        public override string TableName => "BuyTransaction";
    }
}
