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
            
            if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending && string.IsNullOrEmpty(buy.LastTransaction.TransactionHash))
                TransactionBusiness.SetTransactionHash(buy.LastTransaction, transactionHash);
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Error)
                Create(user.Id, buyId, transactionHash);
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending && !string.IsNullOrEmpty(buy.LastTransaction.TransactionHash))
            {
                //TODO: check if transaction exists inside the ethereum node

                TransactionBusiness.ValidateTransaction(transactionHash);
                using (var trans = new TransactionalDapperCommand())
                {
                    buy.LastTransaction.TransactionStatus = TransactionStatus.Lost;
                    buy.LastTransaction.ProcessedDate = DateTime.UtcNow;
                    trans.Update(buy.LastTransaction);
                    InternalCreate(trans, user.Id, buyId, transactionHash);
                    trans.Commit();
                }
            }
            else
                throw new ArgumentException("Invalid transaction status.");
        }

        public void Cancel(string email, int buyId)
        {
            var user = UserBusiness.GetValidUser(email);
            var buy = BuyBusiness.Get(buyId);
            if (buy == null || buy.UserId != user.Id || buy.ExpirationDate.HasValue)
                throw new ArgumentException("Invalid purchase.");

            if (buy.LastTransaction.TransactionStatus == TransactionStatus.Pending &&
                (string.IsNullOrEmpty(buy.LastTransaction.TransactionHash) || true))//TODO: check if transaction exists inside the ethereum node
                TransactionBusiness.Process(buy.LastTransaction, TransactionStatus.Cancel);
            else if (buy.LastTransaction.TransactionStatus == TransactionStatus.Error)
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
    }
}
