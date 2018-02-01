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

            Portfolio portfolio;
            try
            {
                portfolio = PortfolioServices.CreatePortfolio(GetUser(), portfolioRequest.AdvisorId, portfolioRequest.Risk, portfolioRequest.ProjectionValue,
                    portfolioRequest.OptimisticProjection, portfolioRequest.PessimisticProjection,
                    portfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percentage));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = portfolio.Id });
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
        
        protected virtual IActionResult Projection()
        {
            Projections projections;
            try
            {
                projections = PortfolioServices.GetProjections(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(projections);
        }

        protected virtual IActionResult PortfolioHistory()
        {
            List<Auctus.Model.PortfolioHistory> portfolioHistory;
            try
            {
                portfolioHistory = PortfolioServices.ListHistory(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolioHistory);
        }

        protected virtual IActionResult PortfolioDistribution()
        {
            List<Auctus.Model.PortfolioDistribution> portfolioDistribution;
            try
            {
                portfolioDistribution = PortfolioServices.ListPortfolioDistribution(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(portfolioDistribution);
        }
    }
}