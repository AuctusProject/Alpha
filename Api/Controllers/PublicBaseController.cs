using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advisor;
using Api.Model.Portfolio;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class PublicBaseController : BaseController
    {
        protected PublicBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        protected virtual IActionResult Assets()
        {
            var assets = AssetServices.ListAssets();
            return Ok(assets.Select(c => new { id = c.Id, code = c.Code, name = c.Name }));
        }

        protected virtual IActionResult Portfolio(Guid guid)
        {
            var baseValidation = GetBasicValidation(guid, "GetPortfolio");
            if (baseValidation.Return != null)
                return baseValidation.Return;
            
            List<Portfolio> portfolios;
            try
            {
                portfolios = PortfolioServices.ListPortfolio(baseValidation.Email);
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
                            period = p.Advisor.Detail.Period,
                            enabled = p.Advisor.Detail.Enabled
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

        protected virtual IActionResult AdvisorUpdate(Guid guid, int advisorId, AdvisorDetailRequest advisorDetailRequest)
        {
            if (advisorDetailRequest == null)
                return BadRequest();

            var baseValidation = GetBasicValidation(guid, "AdvisorUpdate");
            if (baseValidation.Return != null)
                return baseValidation.Return;

            try
            {
                AdvisorServices.UpdateAdvisor(baseValidation.Email, advisorId, advisorDetailRequest.Description,
                    advisorDetailRequest.Period, advisorDetailRequest.Price, advisorDetailRequest.Enabled);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult Portfolio(Guid guid, PortfolioRequest portfolioRequest)
        {
            if (portfolioRequest == null || portfolioRequest.Distribution == null || portfolioRequest.Distribution.Count == 0)
                return BadRequest();

            var baseValidation = GetBasicValidation(guid, "PostPortfolio");
            if (baseValidation.Return != null)
                return baseValidation.Return;

            Portfolio portfolio;
            try
            {
                portfolio = PortfolioServices.CreatePortfolio(baseValidation.Email, portfolioRequest.AdvisorId, portfolioRequest.Risk, 
                    portfolioRequest.ProjectionValue, portfolioRequest.OptimisticProjection, portfolioRequest.PessimisticProjection,
                    portfolioRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = portfolio.Id });
        }

        protected virtual IActionResult Projection(Guid guid, int portfolioId, ProjectionRequest projectionRequest)
        {
            return new StatusCodeResult(405);
            if (projectionRequest == null || projectionRequest.Distribution == null || projectionRequest.Distribution.Count == 0)
                return BadRequest();

            var baseValidation = GetBasicValidation(guid, "PostProjection");
            if (baseValidation.Return != null)
                return baseValidation.Return;

            try
            {
                PortfolioServices.CreateProjection(baseValidation.Email, portfolioId, projectionRequest.ProjectionValue,
                    projectionRequest.OptimisticProjection, projectionRequest.PessimisticProjection,
                    projectionRequest.Distribution.ToDictionary(c => c.AssetId, c => c.Percent));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult Distribution(Guid guid, int portfolioId, List<DistributionRequest> newDistributionRequest)
        {
            if (newDistributionRequest == null || newDistributionRequest.Count == 0)
                return BadRequest();

            var baseValidation = GetBasicValidation(guid, "PostDistribution");
            if (baseValidation.Return != null)
                return baseValidation.Return;

            try
            {
                PortfolioServices.CreateDistribution(baseValidation.Email, portfolioId, newDistributionRequest.ToDictionary(c => c.AssetId, c => c.Percent));
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

        private BaseValidation GetBasicValidation(Guid guid, string method)
        {
            BaseValidation result = new BaseValidation();
            if (guid == null)
                result.Return = BadRequest();
            else if (!CanAccess(guid, method))
                result.Return = new StatusCodeResult(429);
            else
            {
                var user = GetUser(guid, method);
                if (user == null)
                    result.Return = new StatusCodeResult(403);
                else
                    result.Email = user;
            }
            return result;
        }

        private class BaseValidation
        {
            public IActionResult Return { get; set; }
            public string Email { get; set; }
        }
    }
}