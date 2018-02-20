using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Account
{
    public class WalletBusiness : BaseBusiness<Wallet, WalletData>
    {
        public WalletBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public string Faucet(string address)
        {
            //TODO: call faucet api method and get the transaction hash from return
            return null;
        }
    }
}
