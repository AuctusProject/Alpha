using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class JobBaseController : BaseController
    {
        protected JobBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

        protected virtual IActionResult UpdateAssetsValues()
        {
            RunJobAsync(AssetServices.UpdateAllAssetsValues);
            return Ok();
        }

        protected virtual IActionResult UpdateAssetsCurrentValues()
        {
            RunJobSync(AssetServices.UpdateAllAssetsCurrentValues);
            return Ok();
        }

        //protected virtual IActionResult UpdatePortfoliosHistory()
        //{
        //    RunJobAsync(PortfolioServices.UpdatePortfoliosHistory);
        //    return Ok();
        //}

        protected virtual IActionResult EvaluateEscrowResults()
        {
            RunJobAsync(AdvisorServices.EvaluateEscrowResults);
            return Ok();
        }

        protected virtual IActionResult MakeEscrowResultsTransaction()
        {
            RunJobAsync(AdvisorServices.MakeEscrowResultsTransaction);
            return Ok();
        }

        protected virtual IActionResult CheckPurchasesTransaction()
        {
            RunJobAsync(AdvisorServices.CheckPurchasesTransaction);
            return Ok();
        }

        protected virtual IActionResult CheckEscrowResultsTransaction()
        {
            RunJobAsync(AdvisorServices.CheckEscrowResultsTransactionAsync);
            return Ok();
        }

        private void RunJobAsync(Action action)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    RunJobSync(action);
                }
                catch (Exception e)
                {
                    Logger.LogCritical(e, $"Exception on {action.Method.Name} job");
                }
            });
        }

        private void RunJobSync(Action action)
        {
            Logger.LogInformation($"Job {action.Method.Name} started.");
            action();
            Logger.LogTrace($"Job {action.Method.Name} ended.");
        }
    }
}