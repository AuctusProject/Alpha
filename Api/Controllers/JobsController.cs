using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/jobs/")]
    public class JobsController : BaseController
    {
        public JobsController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }

        [Route("AssetsValues")]
        [HttpPost]
        [Authorize("Bearer")]
        public IActionResult UpdateAssetsValues()
        {
            RunJobAsync(AssetServices.UpdateAllAssetsValues);
            return Ok();
        }

        [Route("PortfoliosHistory")]
        [HttpPost]
        [Authorize("Bearer")]
        public IActionResult UpdatePortfoliosHistory()
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
                    AssetServices.UpdateAllAssetsValues();
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
