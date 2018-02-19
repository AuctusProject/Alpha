using Auctus.DataAccess.Advisor;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Advisor
{
    public class EscrowResultTransactionBusiness : BaseBusiness<EscrowResultTransaction, EscrowResultTransactionData>
    {
        public EscrowResultTransactionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public EscrowResultTransaction SetNew(int escrowResultId, int transactionId)
        {
            var escrowResultTransaction = new EscrowResultTransaction();
            escrowResultTransaction.EscrowResultId = escrowResultId;
            escrowResultTransaction.TransactionId = transactionId;
            return escrowResultTransaction;
        }

        public void MakeTransactions()
        {
            var pendingCreation = EscrowResultBusiness.ListPendingCreation();
            foreach(var pending in pendingCreation)
            {
                try
                {
                    var from = UserBusiness.GetWithWallet(pending.LastTransaction.UserId).Wallet.Address;
                    var value = Math.Max(pending.OwnerTokenResult, pending.BuyerTokenResult);
                    string to;
                    if (pending.OwnerTokenResult > 0)
                        to = UserBusiness.GetOwner(pending.BuyId).Wallet.Address;
                    else
                        to = from;

                    //TODO: make a blockchain transaction with data: from, to, value
                    string transactionHash = null;

                    TransactionBusiness.SetTransactionHash(pending.LastTransaction, transactionHash);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, "Exception on MakeTransactions job");
                }
            }
        }
    }
}
