using Auctus.DataAccess.Advisor;
using Auctus.DataAccess.Core;
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
    public class EscrowResultBusiness : BaseBusiness<EscrowResult, EscrowResultData>
    {
        public EscrowResultBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public List<EscrowResult> ListByPortfolio(int portfolioId)
        {
            return Data.ListByPortfolio(portfolioId);
        }

        public List<EscrowResult> ListPendingCreation()
        {
            return Data.ListPendingCreation();
        }

        public void Evaluate()
        {
            var purchases = BuyBusiness.ListPendingEscrowResult();
            var histories = PortfolioHistoryBusiness.ListHistory(purchases.ToDictionary(c => c.Portfolio.Id, c => c.Portfolio.CreationDate)); 
            foreach (var purchase in purchases)
            {
                try
                {
                    var history = histories.Where(c => c.PortfolioId == purchase.PortfolioId);
                    if (history.Max(c => c.Date) < purchase.ExpirationDate.Value)
                        continue;

                    var projection = ProjectionBusiness.Get(purchase.ProjectionId);
                    var dailyEstimatedPercentage = Util.Util.ConvertMonthlyToDailyRate(projection.ProjectionValue);
                    var estimatedPercentage = dailyEstimatedPercentage.HasValue ? Math.Pow(1.0 + dailyEstimatedPercentage.Value, purchase.Days) - 1.0 : (double?)null;

                    var startDate = purchase.ExpirationDate.Value.AddDays(-purchase.Days);
                    if (startDate < history.Min(c => c.Date))
                        startDate = history.Min(c => c.Date);

                    var historyOfPeriod = history.Where(c => c.Date >= startDate && c.Date <= purchase.ExpirationDate.Value);
                    var historyResult = PortfolioHistoryBusiness.GetHistoryResult(historyOfPeriod);
                    var performedPercentage = historyResult.Value/100.0;

                    decimal buyerTokenAmount = 0, ownerTokenAmount = 0;
                    if (estimatedPercentage > performedPercentage)
                        buyerTokenAmount = purchase.Price;
                    else
                        ownerTokenAmount = purchase.Price;

                    using (var transaction = new TransactionalDapperCommand())
                    {
                        var escrowResult = new EscrowResult()
                        {
                            BuyId = purchase.Id,
                            CreationDate = DateTime.UtcNow,
                            BuyerTokenResult = buyerTokenAmount,
                            OwnerTokenResult = ownerTokenAmount
                        };
                        transaction.Insert(escrowResult);
                        var trans = TransactionBusiness.SetNew(purchase.UserId);
                        transaction.Insert(trans);
                        var escrowResultTrans = EscrowResultTransactionBusiness.SetNew(escrowResult.Id, trans.Id);
                        transaction.Insert(escrowResultTrans);
                        transaction.Commit();
                    }
                }
                catch (Exception ex)
                {
                    Logger.LogCritical(ex, $"Error on evaluate purchase {purchase.Id}");
                }
            }
        }
    }
}
