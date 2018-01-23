﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Advisor;
using Auctus.DomainObjects.Advisor;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class AdvisorBaseController : BaseController
    {
        protected AdvisorBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        protected virtual IActionResult Advisor(AdvisorRequest advisorRequest)
        {
            if (advisorRequest == null)
                return BadRequest();

            Auctus.DomainObjects.Advisor.Advisor advisor;
            try
            {
                advisor = AdvisorServices.CreateAdvisor(GetUser(), advisorRequest.Name, advisorRequest.Description, advisorRequest.Period, advisorRequest.Price);
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
                advisorDetail = AdvisorServices.UpdateAdvisor(GetUser(), advisorId, advisorDetailRequest.Description,
                    advisorDetailRequest.Period, advisorDetailRequest.Price, advisorDetailRequest.Enabled);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { id = advisorDetail.Id });
        }
        
        protected virtual IActionResult Buy(int advisorId)
        {
            try
            {
                AdvisorServices.Buy(GetUser(), advisorId);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }
        
        protected virtual IActionResult ListAdvisors()
        {
            try
            {
                var advisors = AdvisorServices.ListAdvisors(GetUser());
                return Ok(advisors);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
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