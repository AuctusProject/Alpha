using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Portfolio;
using Auctus.DomainObjects.Portfolio;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class PortfolioBaseController : BaseController
    {
        protected PortfolioBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

        protected virtual IActionResult Portfolio(PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || portfolioRequest.Distribution == null)
                return BadRequest();

            Auctus.DomainObjects.Portfolio.Portfolio portfolio;
            try
            {
                portfolio = PortfolioServices.CreatePortfolio(GetUser(), portfolioRequest.AdvisorId, portfolioRequest.Price, portfolioRequest.Name,
                    portfolioRequest.Description, portfolioRequest.ProjectionValue, portfolioRequest.OptimisticProjection, portfolioRequest.PessimisticProjection,
                    portfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percentage));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = portfolio.Id });
        }

        protected virtual IActionResult UpdatePortfolio(int portfolioId, UpdatePortfolioRequest updatePortfolioRequest)
        {
            if (updatePortfolioRequest == null)
                return BadRequest();
            
            try
            {
                PortfolioServices.UpdatePortfolio(GetUser(), portfolioId, updatePortfolioRequest.Price,
                    updatePortfolioRequest.Name, updatePortfolioRequest.Description);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult UpdatePortfolioAndDistribution(int portfolioId, UpdatePortfolioWithDistributionRequest updatePortfolioRequest)
        {
            if (updatePortfolioRequest == null || updatePortfolioRequest.Distribution == null || updatePortfolioRequest.Distribution.Count == 0)
                return BadRequest();

            try
            {
                PortfolioServices.UpdatePortfolioAndDistribution(GetUser(), portfolioId, updatePortfolioRequest.Price, updatePortfolioRequest.Name, 
                    updatePortfolioRequest.Description, updatePortfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percentage));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult DisablePortfolio(int portfolioId)
        {
            try
            {
                PortfolioServices.DisablePortfolio(GetUser(), portfolioId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult Distribution(int portfolioId, List<DistributionRequest> newDistributionRequest)
        {
            if (newDistributionRequest == null || newDistributionRequest.Count == 0)
                return BadRequest();

            try
            {
                PortfolioServices.CreateDistribution(GetUser(), portfolioId, newDistributionRequest.ToDictionary(c => c.AssetId, c => c.Percentage));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult ListPortfolios()
        {
            List<Auctus.Model.Portfolio> portfolios;
            try
            {
                portfolios = PortfolioServices.ListPortfolios(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolios);
        }

        protected virtual IActionResult ListPortfoliosPerformance(DateTime date)
        {
            List<Auctus.Model.Portfolio> portfolios;
            try
            {
                portfolios = PortfolioServices.ListPortfoliosPerformance(GetUser(), date);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolios);
        }

        protected virtual IActionResult ListPurchasedPortfolios()
        {
            List<Auctus.Model.Portfolio> portfolios;
            try
            {
                portfolios = PortfolioServices.ListPurchasedPortfolios(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolios);
        }

        protected virtual IActionResult ListRoboAdvisors([FromQuery]int? goalOption, [FromQuery]int? risk)
        {
            if (!goalOption.HasValue || !risk.HasValue)
                return BadRequest();

            try
            {
                var result = AdvisorServices.ListRoboAdvisors(GetUser(), goalOption.Value, risk.Value);
                return Ok(new { userRisk = result.Key, portfolios = result.Value });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        
        protected virtual IActionResult GetPortfolio(int portfolioId)
        {
            try
            {
                var portfolio = PortfolioServices.GetPortfolio(GetUser(), portfolioId);
                return Ok(portfolio);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        protected virtual IActionResult GetDistribution(int portfolioId)
        {
            try
            {
                var distribution = PortfolioServices.ListPortfolioDistribution(GetUser(), portfolioId);
                return Ok(distribution);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}