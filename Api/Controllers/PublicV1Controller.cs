using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advisor;
using Api.Model.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/public/v1/")]
    [EnableCors("AllowAll")]
    [AllowAnonymous]
    public class PublicV1Controller : PublicBaseController
    {
        public PublicV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

        [Route("assets")]
        [HttpGet]
        public new IActionResult Assets()
        {
            return base.Assets();
        }

        //[Route("{guid}/portfolios")]
        //[HttpGet]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        //public new IActionResult Portfolio([FromQuery]Guid guid)
        //{
        //    return base.Portfolio(guid);
        //}

        //[Route("{guid}/advisors/{advisorId}")]
        //[HttpPatch]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        //public new IActionResult AdvisorUpdate([FromQuery]Guid guid, [FromQuery]int advisorId, [FromBody]AdvisorDetailRequest advisorDetailRequest)
        //{
        //    return base.AdvisorUpdate(guid, advisorId, advisorDetailRequest);
        //}

        //[Route("{guid}/portfolios")]
        //[HttpPost]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[ProducesResponseType(StatusCodes.Status403Forbidden)]
        //[ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        //public new IActionResult Portfolio([FromQuery]Guid guid, [FromBody]PortfolioRequest portfolioRequest)
        //{
        //    return base.Portfolio(guid, portfolioRequest);
        //}

        [Route("{guid}/portfolios/{portfolioId}/projections")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public new IActionResult Projection([FromQuery]Guid guid, [FromQuery]int portfolioId, [FromBody]ProjectionRequest projectionRequest)
        {
            return base.Projection(guid, portfolioId, projectionRequest);
        }

        [Route("{guid}/portfolios/{portfolioId}/distribution")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public new IActionResult Distribution([FromQuery]Guid guid, [FromQuery]int portfolioId, [FromBody]List<DistributionRequest> newDistributionRequest)
        {
            return base.Distribution(guid, portfolioId, newDistributionRequest);
        }

        [Route("check")]
        [HttpGet]
 
        public void Check()
        {
            PortfolioServices.Check();
        }
    }


}