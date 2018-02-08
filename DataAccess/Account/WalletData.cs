using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class WalletData : BaseData<Wallet>
    {
        public override string TableName => "Wallet";
    }
}
