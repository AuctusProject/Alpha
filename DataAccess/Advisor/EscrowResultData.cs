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
    public class EscrowResultData : BaseSQL<EscrowResult>
    {
        public override string TableName => "EscrowResult";

        private const string SELECT_ESCROWRESULT_BY_BUY = @"SELECT e.*, t.* FROM 
                                                            EscrowResult e 
                                                            INNER JOIN Buy b ON b.Id = e.BuyId 
                                                            INNER JOIN EscrowResultTransaction ert ON ert.EscrowResultId = e.Id
                                                            INNER JOIN [Transaction] t ON t.Id = ert.TransactionId
                                                            WHERE b.PortfolioId = @PortfolioId AND
                                                            t.CreationDate = (SELECT max(t2.CreationDate) FROM EscrowResultTransaction ert2 
                                                                                INNER JOIN [Transaction] t2 ON t2.Id = ert2.TransactionId
                                                                                WHERE ert2.EscrowResultId = e.Id)";



        private const string SELECT_PENDING_TRANSACTIONS = @"SELECT e.*, t.* FROM 
                                                            EscrowResult e
                                                            INNER JOIN EscrowResultTransaction et ON et.EscrowResultId = e.Id
                                                            INNER JOIN [Transaction] t ON t.Id = et.TransactionId
                                                            WHERE t.TransactionHash IS NULL AND t.CreationDate > @CreationDate ";

        public List<EscrowResult> ListPendingCreation()
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("CreationDate", DateTime.UtcNow.AddDays(-30), DbType.DateTime);
            return Query<EscrowResult, Transaction, EscrowResult>(SELECT_PENDING_TRANSACTIONS,
                                (er, trans) =>
                                {
                                    er.LastTransaction = trans;
                                    return er;
                                }, "Id", parameters).ToList();
        }

        public List<EscrowResult> ListByPortfolio(int portfolioId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);
            return Query<EscrowResult, Transaction, EscrowResult>(SELECT_ESCROWRESULT_BY_BUY,
                            (er, trans) =>
                            {
                                er.LastTransaction = trans;
                                return er;
                            }, "Id", parameters).ToList();
        }
    }
}
