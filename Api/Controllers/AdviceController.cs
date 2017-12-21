using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advice;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/advice/")]
    public class AdviceController : BaseController
    {
        public AdviceController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        [Route("advisor")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Advisor([FromBody]AdvisorRequest advisorRequest)
        {
            if (advisorRequest == null)
                return BadRequest();

            Advisor advisor;
            try
            {
                advisor = AdviceService.CreateAdvisor(GetUser(), advisorRequest.Name, advisorRequest.Description, advisorRequest.Period, advisorRequest.Price);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = advisor.Id });
        }

        [Route("advisor")]
        [HttpPatch]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AdvisorUpdate([FromBody]AdvisorDetailRequest advisorDetailRequest)
        {
            if (advisorDetailRequest == null)
                return BadRequest();

            AdvisorDetail advisorDetail;
            try
            {
                advisorDetail = AdviceService.UpdateAdvisor(GetUser(), advisorDetailRequest.IdAdvisor, advisorDetailRequest.Description,
                    advisorDetailRequest.Period, advisorDetailRequest.Price, advisorDetailRequest.Enabled);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = advisorDetail.Id });
        }

        [Route("portfolio")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Portfolio([FromBody]PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || portfolioRequest.Distribution == null)
                return BadRequest();

            Portfolio portfolio;
            try
            {
                portfolio = AdviceService.CreatePortfolio(GetUser(), portfolioRequest.AdvisorId, portfolioRequest.Risk, portfolioRequest.ProjectionValue,
                    portfolioRequest.OptimisticProjection, portfolioRequest.PessimisticProjection,
                    portfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = portfolio.Id });
        }

        [Route("disbleportfolio")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DisablePortfolio([FromBody]int portfolioId)
        {
            try
            {
                AdviceService.DisablePortfolio(GetUser(), portfolioId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("distribution")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Distribution([FromBody]NewDistributionRequest newDistributionRequest)
        {
            if (newDistributionRequest == null || newDistributionRequest.Distribution == null)
                return BadRequest();

            try
            {
                AdviceService.CreateDistribution(GetUser(), newDistributionRequest.PortfolioId, 
                    newDistributionRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("buy")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Buy([FromBody]int advisorId)
        {
            try
            {
                AdviceService.Buy(GetUser(), advisorId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }
    }
}