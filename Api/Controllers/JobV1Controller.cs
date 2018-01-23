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
    [Route("api/jobs/v1/")]
    [Authorize("Bearer")]
    public class JobV1Controller : JobBaseController
    {
        public JobV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }

        [Route("assets/values")]
        [HttpPost]
        public new IActionResult UpdateAssetsValues()
        {
            return base.UpdateAssetsValues();
        }

        [Route("portfolios/history")]
        [HttpPost]
        public new IActionResult UpdatePortfoliosHistory()
        {
            return base.UpdatePortfoliosHistory();
        }
    }
}
