using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Auctus.Business.Account
{
    public class WalletBusiness : BaseBusiness<Wallet, WalletData>
    {
        public WalletBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public string Faucet(string address)
        {
            return Web3.Web3Business.FaucetTransaction(address).TransactionHash;
        }

        public bool IsValidAddress(string address)
        {
            return !string.IsNullOrEmpty(address) && Regex.IsMatch(address, "^(0x)?[0-9a-f]{40}$", RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        }
    }
}
