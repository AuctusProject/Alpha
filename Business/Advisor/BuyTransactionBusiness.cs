using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Advisor
{
    public class BuyTransactionBusiness : BaseBusiness<BuyTransaction, BuyTransactionData>
    {
        public BuyTransactionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public BuyTransaction SetNew(int buyId, int transactionId)
        {
            var buyTransaction = new BuyTransaction();
            buyTransaction.BuyId = buyId;
            buyTransaction.TransactionId = transactionId;
            return buyTransaction;
        }

        public void SetTransactionHash(string email, int buyId, string transactionHash)
        {
            var user = UserBusiness.GetValidUser(email);
            var buy = BuyBusiness.Get(buyId);
            if (buy == null || buy.UserId != user.Id || buy.ExpirationDate.HasValue)
                throw new ArgumentException("Invalid purchase.");
            
            if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending.Value && string.IsNullOrEmpty(buy.LastTransaction.TransactionHash))
                TransactionBusiness.SetTransactionHash(buy.LastTransaction, transactionHash);
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Error.Value)
                Create(user.Id, buyId, transactionHash);
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending.Value && TransactionBusiness.TransactionCanBeConsideredLost(buy.LastTransaction) 
                && buy.LastTransaction.TransactionHash != transactionHash.ToLower().Trim())
            {
                try
                {
                    var transaction = Web3.Web3Business.CheckTransaction(buy.LastTransaction.TransactionHash);
                    throw new ArgumentException(transaction.BlockNumber.HasValue ? "Before transaction was processed." : "Before transaction is still pending.");
                }
                catch (DomainObjects.Web3.Web3Exception ex)
                {
                    if (ex.Code == 404)
                        HandleLostTransaction(buy.LastTransaction, user.Id, buyId, transactionHash);
                    else
                        throw;
                }
            }
            else
                throw new ArgumentException("Invalid transaction status.");
        }

        public List<Model.Portfolio.Distribution> CheckTransactionHash(string email, int buyId, string transactionHash)
        {
            var user = UserBusiness.GetValidUser(email);
            var buy = BuyBusiness.Get(buyId);
            if (buy == null || buy.UserId != user.Id || buy.ExpirationDate.HasValue || buy.LastTransaction == null 
                || buy.LastTransaction.TransactionStatus != TransactionStatus.Pending.Value)
                throw new ArgumentException("Invalid purchase.");

            var status = CheckAndProcessTransaction(buy.LastTransaction, buy.Id);
            if (status == TransactionStatus.Success)
            {
                var portfolio = PortfolioBusiness.GetSimple(buy.PortfolioId);
                return DistributionBusiness.ListByProjection(portfolio.ProjectionId.Value);
            }
            else
                return null;
        }

        public void Cancel(string email, int buyId)
        {
            var user = UserBusiness.GetValidUser(email);
            var buy = BuyBusiness.Get(buyId);
            if (buy == null || buy.UserId != user.Id || buy.ExpirationDate.HasValue)
                throw new ArgumentException("Invalid purchase.");

            if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending.Value)
            {
                if (string.IsNullOrEmpty(buy.LastTransaction.TransactionHash))
                    TransactionBusiness.Process(buy.LastTransaction, TransactionStatus.Cancel);
                else
                {
                    try
                    {
                        var transaction = Web3.Web3Business.CheckTransaction(buy.LastTransaction.TransactionHash);
                        throw new ArgumentException(transaction.BlockNumber.HasValue ? "The transaction is already minted." : "Pending transaction cannot be canceled.");
                    }
                    catch (DomainObjects.Web3.Web3Exception ex)
                    {
                        if (ex.Code == 404)
                            TransactionBusiness.Process(buy.LastTransaction, TransactionStatus.Cancel);
                        else
                            throw;
                    }
                }
            }
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Error.Value)
            {
                using (var transaction = new TransactionalDapperCommand())
                {
                    var trans = TransactionBusiness.SetCanceled(user.Id);
                    transaction.Insert(trans);
                    var buyTrans = BuyTransactionBusiness.SetNew(buyId, trans.Id);
                    transaction.Insert(buyTrans);
                    transaction.Commit();
                }
            }
            else
                throw new ArgumentException("Invalid transaction status.");
        }

        private void Create(int userId, int buyId, string transactionHash)
        {
            TransactionBusiness.ValidateTransaction(transactionHash);
            using (var transaction = new TransactionalDapperCommand())
            {
                InternalCreate(transaction, userId, buyId, transactionHash);
                transaction.Commit();
            }
        }

        private void InternalCreate(TransactionalDapperCommand  transaction, int userId, int buyId, string transactionHash)
        {
            var trans = TransactionBusiness.SetNew(userId, transactionHash);
            transaction.Insert(trans);
            var buyTrans = BuyTransactionBusiness.SetNew(buyId, trans.Id);
            transaction.Insert(buyTrans);
        }

        private TransactionStatus CheckAndProcessTransaction(Transaction buyTransaction, int buyId)
        {
            TransactionStatus status = TransactionStatus.Pending;
            try
            {
                var transaction = Web3.Web3Business.CheckTransaction(buyTransaction.TransactionHash, "Escrow(address,uint256)");
                if (transaction.BlockNumber.HasValue)
                {
                    if (transaction.Status.Value == 1)
                    {
                        if (transaction.EventData != null && transaction.EventData.Length == 2 && 
                            !string.IsNullOrEmpty(transaction.EventData[0]) && !string.IsNullOrEmpty(transaction.EventData[1]))
                        {
                            var wallet = WalletBusiness.GetByUser(buyTransaction.UserId);
                            if (transaction.EventData[0].ToLower().Trim() != wallet.Address)
                                status = TransactionStatus.Fraud;
                            else
                            {
                                var purchase = BuyBusiness.GetSimple(buyId);
                                var escrowedValue = Util.Util.ConvertBigNumber(transaction.EventData[1], 18);
                                if (purchase.Price > escrowedValue)
                                    status = TransactionStatus.Fraud;
                                else
                                {
                                    var transactions = ListByUserAndHash(buyTransaction.UserId, buyTransaction.TransactionHash);
                                    if (transactions.Count > 1 && (transactions.Any(c => c.BuyId != buyId) 
                                        || transactions.Any(c => c.Transaction.TransactionStatus != TransactionStatus.Pending.Value)))
                                        status = TransactionStatus.Fraud;
                                    else
                                    {
                                        status = TransactionStatus.Success;
                                        using (var trans = new TransactionalDapperCommand())
                                        {
                                            purchase.ExpirationDate = DateTime.UtcNow.Date.AddDays(purchase.Days);
                                            trans.Update(purchase);
                                            buyTransaction.TransactionStatus = status.Value;
                                            buyTransaction.ProcessedDate = DateTime.UtcNow;
                                            trans.Update(buyTransaction);
                                            trans.Commit();
                                        }
                                    }
                                }
                            }
                        }
                        else
                            status = TransactionStatus.Fraud;
                    }
                    else
                        status = TransactionStatus.Error;

                    if (status != TransactionStatus.Success)
                        TransactionBusiness.Process(buyTransaction, status);
                }
            }
            catch (DomainObjects.Web3.Web3Exception ex)
            {
                if (ex.Code == 404 && TransactionBusiness.TransactionCanBeConsideredLost(buyTransaction))
                    HandleLostTransaction(buyTransaction, buyTransaction.UserId, buyId, null);
                else
                    throw;
            }
            return status;
        }

        public void CheckTransactions()
        {
            var pendingTransactions = Data.ListPendingTransactions();
            foreach (var pending in pendingTransactions)
            {
                try
                {
                    CheckAndProcessTransaction(pending.Transaction, pending.BuyId);
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, $"Exception on check buy transaction {pending.TransactionId}");
                }
            }
        }
        
        private void HandleLostTransaction(Transaction transaction, int userId, int buyId, string transactionHash)
        {
            using (var trans = new TransactionalDapperCommand())
            {
                transaction.TransactionStatus = TransactionStatus.Lost.Value;
                transaction.ProcessedDate = DateTime.UtcNow;
                trans.Update(transaction);
                InternalCreate(trans, userId, buyId, transactionHash);
                trans.Commit();
            }
        }

        private List<BuyTransaction> ListByUserAndHash(int userId, string transactionHash)
        {
            return Data.ListByUserAndHash(userId, transactionHash);
        }
    }
}
