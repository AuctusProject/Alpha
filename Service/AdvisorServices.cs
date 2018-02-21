using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class AdvisorServices : BaseServices
    {
        public AdvisorServices(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Advisor CreateAdvisor(string email, string name, string description)
        {
            return AdvisorBusiness.Create(email, name, description);
        }

        public AdvisorDetail UpdateAdvisor(string email, int advisorId, string name, string description)
        {
            return AdvisorDetailBusiness.Create(email, advisorId, name, description);
        }

        public AdvisorDetail DisableAdvisor(string email, int advisorId)
        {
            return AdvisorDetailBusiness.Disable(email, advisorId);
        }

        public Model.Advisor ListAdvisorDetails(string email, int advisorId)
        {
            return AdvisorBusiness.ListDetails(email, advisorId);
        }

        public Buy Buy(string email, string address, int portfolioId, int days, int? goalOptionId, int? timeframe, 
            int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            return BuyBusiness.Create(email, address, portfolioId, days, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public void SetBuyTransaction(string email, int buyId, string transactionHash)
        {
            BuyTransactionBusiness.SetTransactionHash(email, buyId, transactionHash);
        }

        public void CancelBuyTransaction(string email, int buyId)
        {
            BuyTransactionBusiness.Cancel(email, buyId);
        }

        public KeyValuePair<int, IEnumerable<Model.Portfolio>> ListRoboAdvisors(string email, int goalOptionId, int risk)
        {
            return AdvisorBusiness.ListRoboAdvisors(email, goalOptionId, risk);
        }

        public void EvaluateEscrowResults()
        {
            EscrowResultBusiness.Evaluate();
        }

        public void MakeEscrowResultsTransaction()
        {
            EscrowResultTransactionBusiness.MakeTransactions();
        }

        public void CheckPurchasesTransaction()
        {
            BuyTransactionBusiness.CheckTransactions();
        }

        public async void CheckEscrowResultsTransactionAsync()
        {
            await EscrowResultTransactionBusiness.CheckTransactionsAsync();
        }
    }
}
