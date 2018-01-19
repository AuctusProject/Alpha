using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advice;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advice;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;

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

            Auctus.DomainObjects.Advice.Advisor advisor;
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

        [Route("disableportfolio")]
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

        [Route("projections")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Projection()
        {
            Projections projections;
            try
            {
                projections = AdviceService.GetProjections(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(projections);
        }

        [Route("portfolio/history")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PortfolioHistory()
        {
            List<Auctus.Model.PortfolioHistory> portfolioHistory;
            try
            {
                portfolioHistory = AdviceService.ListHistory(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolioHistory);
        }

        [Route("portfolio/distribution")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PortfolioDistribution()
        {
            List<Auctus.Model.PortfolioDistribution> portfolioDistribution;
            try
            {
                portfolioDistribution = AdviceService.ListPortfolioDistribution(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolioDistribution);
        }

        [Route("advisors")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListAdvisors()
        {
            try
            {
                var advisors = AdviceService.ListAdvisors(GetUser());
                return Ok(advisors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [Route("advisors/{advisorId}/details")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListAdvisorDetails([FromRoute]int advisorId)
        {
            try
            {
                var advisor = AdviceService.ListAdvisorDetails(GetUser(), advisorId);
                return Ok(advisor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}