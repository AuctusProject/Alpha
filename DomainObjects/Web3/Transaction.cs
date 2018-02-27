using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Web3
{
    public class Transaction
    {
        public string TransactionHash { get; set; }
        public int? BlockNumber { get; set; }
        public string BlockHash { get; set; }
        public string ContractAddress { get; set; }
        public int? Status { get; set; }
        public string[] EventData { get; set; }
    }
}
