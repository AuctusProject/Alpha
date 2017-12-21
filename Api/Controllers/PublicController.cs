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
            var method = "GetPortfolio";
            if (guid == null)
                return BadRequest();
            if (!CanAccess(guid, method))
                return new StatusCodeResult(429);
            var user = GetUser(guid, method);
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
            return Ok(portfolios.Select(p => 
                    new
                    {
                        id = p.Id,
                        risk = p.Risk,
                        projectionValue = p.Projection.ProjectionValue,
                        pessimisticValue = p.Projection.PessimisticProjection,
                        optimisticValue = p.Projection.OptimisticProjection,
                        advisor = new
                        {
                            id = p.AdvisorId,
                            name = p.Advisor.Name,
                            price = p.Advisor.Detail.Price,
                            description = p.Advisor.Detail.Description,
                            period = p.Advisor.Detail.Period
                        },
                        distribution = p.Projection.Distribution.Select(d => new
                        {
                            asset = new
                            {
                                id = d.Asset.Id,
                                name = d.Asset.Name,
                                code = d.Asset.Code
                            },
                            percent = d.Percent
                        })
                    }));
        }

        [Route("advisor/{guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult AdvisorUpdate([FromQuery]Guid guid, [FromBody]AdvisorDetailRequest advisorDetailRequest)
        {
            var method = "AdvisorUpdate";
            if (advisorDetailRequest == null || guid == null)
                return BadRequest();
            if (!CanAccess(guid, method))
                return new StatusCodeResult(429);
            var user = GetUser(guid, method);
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
            var method = "PostPortfolio";
            if (portfolioRequest == null || portfolioRequest.Distribution == null || guid == null)
                return BadRequest();
            if (!CanAccess(guid, method))
                return new StatusCodeResult(429);
            var user = GetUser(guid, method);
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
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public IActionResult Projection([FromQuery]Guid guid, [FromBody]ProjectionRequest projectionRequest)
        {
            return new StatusCodeResult(405);
            var method = "PostProjection";
            if (projectionRequest == null || projectionRequest.Distribution == null || !projectionRequest.PortfolioId.HasValue || guid == null)
                return BadRequest();
            if (!CanAccess(guid, method))
                return new StatusCodeResult(429);
            var user = GetUser(guid, method);
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

        [Route("distribution/{guid}")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public IActionResult Distribution([FromQuery]Guid guid, [FromBody]NewDistributionRequest newDistributionRequest)
        {
            var method = "PostDistribution";
            if (newDistributionRequest == null || newDistributionRequest.Distribution == null)
                return BadRequest();
            if (!CanAccess(guid, method))
                return new StatusCodeResult(429);
            var user = GetUser(guid, method);
            if (user == null)
                return new StatusCodeResult(403);

            try
            {
                AdviceService.CreateDistribution(user, newDistributionRequest.PortfolioId, 
                    newDistributionRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        private string GetCacheKey(Guid guid, string method)
        {
            return string.Format("{0}_{1}", guid.ToString(), method);
        }

        private bool CanAccess(Guid guid, string method)
        {
            return MemoryCache.Get<string>(GetCacheKey(guid, method)) == null;
        }

        private string GetUser(Guid guid, string method)
        {
            User user = AccountServices.GetUser(guid);
            if (user != null)
            {
                MemoryCache.Set<string>(GetCacheKey(guid, method), user.Email, 10);
                return user.Email;
            }
            else
                return null;
        }
    }
}