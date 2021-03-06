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
using Microsoft.AspNetCore.NodeServices;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/advisors/v1/")]
    [Authorize("Bearer")]
    [EnableCors("Default")]
    public class AdvisorV1Controller : AdvisorBaseController
    {
        public AdvisorV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

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

        [Route("{advisorId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult AdvisorDisable([FromRoute]int advisorId)
        {
            return base.AdvisorDisable(advisorId);
        }

        [Route("purchases")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Buy([FromBody]BuyRequest buyRequest)
        {
            return base.Buy(buyRequest);
        }

        [Route("purchases/{buyId}/transaction")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult SetBuyTransaction([FromRoute]int buyId, [FromBody]BuyTransactionRequest buyTransactionRequest)
        {
            return base.SetBuyTransaction(buyId, buyTransactionRequest);
        }

        [Route("purchases/{buyId}")]
        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult CancelBuyTransaction([FromRoute]int buyId)
        {
            return base.CancelBuyTransaction(buyId);
        }

        [Route("purchases/{buyId}/check")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult CheckBuyTransaction([FromRoute]int buyId)
        {
            return base.CheckBuyTransaction(buyId);
        }

        [AllowAnonymous]
        [Route("{advisorId}")]
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult ListAdvisorDetails([FromRoute]int advisorId)
        {
            return base.ListAdvisorDetails(advisorId);
        }
    }
}