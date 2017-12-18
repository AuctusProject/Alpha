using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advice;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/public/")]
    [EnableCors("AllowAll")]
    [AllowAnonymous]
    public class PublicController : BaseController
    {
        public PublicController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }

        [Route("assets")]
        [HttpGet]
        public IActionResult Assets()
        {
            var assets = AssetServices.ListAssets();
            return Ok(assets.Select(c => new { id = c.Id, code = c.Code, name = c.Name }));
        }

        [Route("portfolio/{guid}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Portfolio([FromQuery]Guid guid)
        {
            if (guid == null)
                return BadRequest();
            if (!CanAccess(guid))
                return new StatusCodeResult(429);
            var user = GetUser(guid);
            if (user == null)
                return new StatusCodeResult(403);

            List<Portfolio> portfolios;
            try
            {
                portfolios = AdviceService.ListPortfolio(user);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolios);
        }

        [Route("advisor/{guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult AdvisorUpdate([FromQuery]Guid guid, [FromBody]AdvisorDetailRequest advisorDetailRequest)
        {
            if (advisorDetailRequest == null || guid == null)
                return BadRequest();
            if (!CanAccess(guid))
                return new StatusCodeResult(429);
            var user = GetUser(guid);
            if (user == null)
                return new StatusCodeResult(403);

            try
            {
                AdviceService.UpdateAdvisor(user, advisorDetailRequest.IdAdvisor, advisorDetailRequest.Description,
                    advisorDetailRequest.Period, advisorDetailRequest.Price, advisorDetailRequest.Enabled);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("portfolio/{guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Portfolio([FromQuery]Guid guid, [FromBody]PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || portfolioRequest.Distribution == null || guid == null)
                return BadRequest();
            if (!CanAccess(guid))
                return new StatusCodeResult(429);
            var user = GetUser(guid);
            if (user == null)
                return new StatusCodeResult(403);

            Portfolio portfolio;
            try
            {
                portfolio = AdviceService.CreatePortfolio(user, portfolioRequest.AdvisorId, portfolioRequest.Risk, portfolioRequest.ProjectionValue,
                    portfolioRequest.OptimisticProjection, portfolioRequest.PessimisticProjection,
                    portfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = portfolio.Id });
        }

        [Route("projection/{guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Projection([FromQuery]Guid guid, [FromBody]ProjectionRequest projectionRequest)
        {
            if (projectionRequest == null || projectionRequest.Distribution == null || !projectionRequest.PortfolioId.HasValue || guid == null)
                return BadRequest();
            if (!CanAccess(guid))
                return new StatusCodeResult(429);
            var user = GetUser(guid);
            if (user == null)
                return new StatusCodeResult(403);

            try
            {
                AdviceService.CreateProjection(user, projectionRequest.PortfolioId.Value, projectionRequest.ProjectionValue, 
                    projectionRequest.OptimisticProjection, projectionRequest.PessimisticProjection,
                    projectionRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        private bool CanAccess(Guid guid)
        {
            return MemoryCache.Get<string>(guid.ToString()) == null;
        }

        private string GetUser(Guid guid)
        {
            User user = AccountServices.GetUser(guid);
            if (user != null)
            {
                MemoryCache.Set<string>(guid.ToString(), user.Email, 5);
                return user.Email;
            }
            else
                return null;
        }
    }
}