using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advisor;
using Auctus.DomainObjects.Advisor;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class AdvisorBaseController : BaseController
    {
        protected AdvisorBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

        protected virtual IActionResult Advisor(AdvisorRequest advisorRequest)
        {
            if (advisorRequest == null)
                return BadRequest();

            Auctus.DomainObjects.Advisor.Advisor advisor;
            try
            {
                advisor = AdvisorServices.CreateAdvisor(GetUser(), advisorRequest.Name, advisorRequest.Description);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = advisor.Id });
        }

        protected virtual IActionResult AdvisorUpdate(int advisorId, AdvisorDetailRequest advisorDetailRequest)
        {
            if (advisorDetailRequest == null)
                return BadRequest();

            AdvisorDetail advisorDetail;
            try
            {
                advisorDetail = AdvisorServices.UpdateAdvisor(GetUser(), advisorId, advisorDetailRequest.Name, advisorDetailRequest.Description);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = advisorDetail.Id });
        }

        protected virtual IActionResult AdvisorDisable(int advisorId)
        {
            if (advisorId == 0)
                return BadRequest();
            
            try
            {
                AdvisorServices.DisableAdvisor(GetUser(), advisorId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult Buy(BuyRequest buyRequest)
        {
            if (buyRequest == null)
                return BadRequest();

            Buy buy;
            try
            {
                buy = AdvisorServices.Buy(GetUser(), buyRequest.Address, buyRequest.PortfolioId, buyRequest.Days, buyRequest.Goal?.GoalOptionId,
                        buyRequest.Goal?.Timeframe, buyRequest.Goal?.Risk, buyRequest.Goal?.TargetAmount, buyRequest.Goal?.StartingAmount,
                        buyRequest.Goal?.MonthlyContribution);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = buy.Id });
        }

        protected virtual IActionResult SetBuyTransaction(int buyId, BuyTransactionRequest buyTransactionRequest)
        {
            if (buyTransactionRequest == null || buyId == 0)
                return BadRequest();
            
            try
            {
                AdvisorServices.SetBuyTransaction(GetUser(), buyId, buyTransactionRequest.TransactionHash);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult CancelBuyTransaction(int buyId)
        {
            if (buyId == 0)
                return BadRequest();

            try
            {
                AdvisorServices.CancelBuyTransaction(GetUser(), buyId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult ListAdvisorDetails(int advisorId)
        {
            try
            {
                var advisor = AdvisorServices.ListAdvisorDetails(GetUser(), advisorId);
                return Ok(advisor);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}