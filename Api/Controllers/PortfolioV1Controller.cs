using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/portfolios/v1/")]
    [Authorize("Bearer")]
    public class PortfolioV1Controller : PortfolioBaseController
    {
        public PortfolioV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Portfolio([FromBody]PortfolioRequest portfolioRequest)
        {
            return base.Portfolio(portfolioRequest);
        }

        [Route("{portfolioId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult DisablePortfolio([FromRoute]int portfolioId)
        {
            return base.DisablePortfolio(portfolioId);
        }

        [Route("{portfolioId}/distribution")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Distribution([FromRoute]int portfolioId, [FromBody]List<DistributionRequest> newDistributionRequest)
        {
            return base.Distribution(portfolioId, newDistributionRequest);
        }
        
        [Route("projections")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Projection()
        {
            return base.Projection();
        }

        [Route("history")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult PortfolioHistory()
        {
            return base.PortfolioHistory();
        }

        [Route("distribution")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult PortfolioDistribution()
        {
            return base.PortfolioDistribution();
        }
    }
}