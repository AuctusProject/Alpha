using Auctus.DomainObjects.Advisor;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class EscrowResultTransactionData : BaseData<EscrowResultTransaction>
    {
        public override string TableName => "EscrowResultTransaction";
    }
}
