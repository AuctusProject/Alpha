using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class BuyTransactionData : BaseData<BuyTransaction>
    {
        public override string TableName => "BuyTransaction";

        private const string SELECT_PENDING_TRANSACTION = @"SELECT bt.*, t.* FROM 
                                                            BuyTransaction bt 
                                                            INNER JOIN [Transaction] t ON t.Id = bt.TransactionId
                                                            WHERE t.TransactionHash IS NOT NULL AND t.TransactionStatus = 0 AND t.CreationDate > @CreationDate";

        public List<BuyTransaction> ListPendingTransactions()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CreationDate", DateTime.UtcNow.AddDays(-7), DbType.DateTime);
            return Query<BuyTransaction, Transaction, BuyTransaction>(SELECT_PENDING_TRANSACTION,
                            (buy, trans) =>
                            {
                                buy.Transaction = trans;
                                return buy;
                            }, "Id", parameters).ToList();
        }
    }
}
