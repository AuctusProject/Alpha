﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Cors;
using Api.Model.Advisor;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/advisors/v1/")]
    [Authorize("Bearer")]
    public class AdvisorV1Controller : AdvisorBaseController
    {
        public AdvisorV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Advisor([FromBody]AdvisorRequest advisorRequest)
        {
            return base.Advisor(advisorRequest);
        }

        [Route("{advisorId}")]
        [HttpPatch]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult AdvisorUpdate([FromRoute]int advisorId, [FromBody]AdvisorDetailRequest advisorDetailRequest)
        {
            return base.AdvisorUpdate(advisorId, advisorDetailRequest);
        }
        
        [Route("purchase")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Buy([FromBody]int advisorId)
        {
            return base.Buy(advisorId);
        }
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult ListAdvisors()
        {
            return base.ListAdvisors();
        }

        [Route("{advisorId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult ListAdvisorDetails([FromRoute]int advisorId)
        {
            return base.ListAdvisorDetails(advisorId);
        }
    }
}