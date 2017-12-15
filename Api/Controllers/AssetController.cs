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
    [Route("api/asset/")]
    public class AssetController : BaseController
    {
        public AssetController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base (loggerFactory, cache, serviceProvider) { }
        
        [Route("updatevalue")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult updatevalue()
        {
            AssetServices.UpdateAllAssetsValues();
            return Ok();
        }

        [Route("updateportfolioshistory")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult updatePortfoliosHistory()
        {
            PortfolioServices.UpdatePortfoliosHistory();
            return Ok();
        }
    }
}
