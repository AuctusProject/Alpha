using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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

                    var transactionHash = Web3.Web3Business.MakeEscrowResultTransaction(from, to, value).TransactionHash;
                    TransactionBusiness.SetTransactionHash(pending.LastTransaction, transactionHash);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, $"Exception on make escrow result transaction for {pending.Id}");
                }
            }
        }

        public async Task CheckTransactionsAsync()
        {
            var pendingTransactions = Data.ListPendingTransactions();
            foreach (var pending in pendingTransactions)
            {
                try
                {
                    var transaction = Web3.Web3Business.CheckTransaction(pending.Transaction.TransactionHash);
                    if (transaction.BlockNumber.HasValue)
                    {
                        if (transaction.Status.Value == 1)
                            TransactionBusiness.Process(pending.Transaction, TransactionStatus.Success);
                        else
                        {
                            TransactionBusiness.Process(pending.Transaction, TransactionStatus.Error);
                            await Email.SendErrorEmailAsync($"Error on escrow result transaction {pending.TransactionId}, with hash {pending.Transaction.TransactionHash}.");
                        }
                    }
                }
                catch (DomainObjects.Web3.Web3Exception ex)
                {
                    if (ex.Code == 404 && TransactionBusiness.TransactionCanBeConsideredLost(pending.Transaction))
                        HandleLostTransaction(pending.Transaction, pending.Transaction.UserId, pending.EscrowResultId);
                    else
                        throw;
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, $"Exception on check escrow result transaction {pending.TransactionId}");
                }
            }
        }

        private void HandleLostTransaction(Transaction transaction, int userId, int escrowResultId)
        {
            using (var trans = new TransactionalDapperCommand())
            {
                transaction.TransactionStatus = TransactionStatus.Lost.Value;
                transaction.ProcessedDate = DateTime.UtcNow;
                trans.Update(transaction);
                var newTransaction = TransactionBusiness.SetNew(userId);
                trans.Insert(newTransaction);
                var escrowResultTrans = SetNew(escrowResultId, newTransaction.Id);
                trans.Insert(escrowResultTrans);
                trans.Commit();
            }
        }
    }
}
