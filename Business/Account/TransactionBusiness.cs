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
    public class TransactionBusiness : BaseBusiness<Transaction, TransactionData>
    {
        public TransactionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Transaction SetNew(int userId, string transactionHash = null)
        {
            var transaction = BaseCreation(userId, transactionHash);
            transaction.TransactionStatus = TransactionStatus.Pending;
            return transaction;
        }

        public Transaction SetCanceled(int userId)
        {
            var transaction = BaseCreation(userId, null);
            transaction.TransactionStatus = TransactionStatus.Cancel;
            transaction.ProcessedDate = DateTime.UtcNow;
            return transaction;
        }

        public Transaction SetTransactionHash(Transaction transaction, string transactionHash)
        {
            ValidateTransaction(transactionHash);
            transaction.TransactionHash = transactionHash;
            Data.Update(transaction);
            return transaction;
        }

        public Transaction Process(Transaction transaction, TransactionStatus transactionStatus)
        {
            transaction.TransactionStatus = transactionStatus;
            transaction.ProcessedDate = DateTime.UtcNow;
            Data.Update(transaction);
            return transaction;
        }

        public void ValidateTransaction(string transactionHash)
        {
            if (string.IsNullOrWhiteSpace(transactionHash))
                throw new ArgumentException("Invalid transaction hash.");
        }

        private Transaction BaseCreation(int userId, string transactionHash)
        {
            var transaction = new Transaction();
            transaction.CreationDate = DateTime.UtcNow;
            transaction.UserId = userId;
            transaction.TransactionHash = transactionHash;
            return transaction;
        }
    }
}
