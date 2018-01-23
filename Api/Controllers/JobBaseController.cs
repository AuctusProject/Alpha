using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class JobBaseController : BaseController
    {
        protected JobBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }

        protected virtual IActionResult UpdateAssetsValues()
        {
            RunJobAsync(AssetServices.UpdateAllAssetsValues);
            return Ok();
        }

        protected virtual IActionResult UpdatePortfoliosHistory()
        {
            RunJobAsync(PortfolioServices.UpdatePortfoliosHistory);
            return Ok();
        }

        private void RunJobAsync(Action action)
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    Logger.LogTrace($"Job {action.Method.Name} started.");
                    action();
                    Logger.LogTrace($"Job {action.Method.Name} ended.");
                }
                catch (Exception e)
                {
                    Logger.LogError(e, $"Exception on {action.Method.Name} job");
                }
            });
        }
    }
}