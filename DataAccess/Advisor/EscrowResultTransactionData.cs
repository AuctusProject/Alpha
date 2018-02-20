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
    public class EscrowResultTransactionData : BaseData<EscrowResultTransaction>
    {
        public override string TableName => "EscrowResultTransaction";

        private const string SELECT_PENDING_TRANSACTION = @"SELECT et.*, t.* FROM 
                                                            EscrowResultTransaction et 
                                                            INNER JOIN [Transaction] t ON t.Id = et.TransactionId
                                                            WHERE t.TransactionHash IS NOT NULL AND t.TransactionStatus = 0 AND t.CreationDate > @CreationDate";

        public List<EscrowResultTransaction> ListPendingTransactions()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CreationDate", DateTime.UtcNow.AddDays(-7), DbType.DateTime);
            return Query<EscrowResultTransaction, Transaction, EscrowResultTransaction>(SELECT_PENDING_TRANSACTION,
                            (er, trans) =>
                            {
                                er.Transaction = trans;
                                return er;
                            }, "Id", parameters).ToList();
        }
    }
}
