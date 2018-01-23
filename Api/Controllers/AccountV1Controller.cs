﻿using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Api.Controllers
{
    [Produces("application/json")]
    [Route("api/accounts/v1/")]
    public class AccountV1Controller : AccountBaseController
    {
        public AccountV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base (loggerFactory, cache, serviceProvider) { }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            return base.Login(loginRequest);
        }
        
        [Route("registration/simple")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SimpleRegister([FromBody]SimpleRegisterRequest registerRequest)
        {
            return await base.SimpleRegister(registerRequest);
        }

        [Route("registration/full")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> FullRegister([FromBody]FullRegisterRequest registerRequest)
        {
            return await base.FullRegister(registerRequest);
        }

        [Route("password/forgotten")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> ForgotPassword([FromBody]string email)
        {
            return await base.ForgotPassword(email);
        }

        [Route("confirmation")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult ConfirmEmail([FromBody]ConfirmEmailRequest confirmEmailRequest)
        {
            return base.ConfirmEmail(confirmEmailRequest);
        }

        [Route("password/change")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult ChangePassword([FromBody]string password)
        {
            return base.ChangePassword(password);
        }

        [Route("goals")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult SetGoal([FromBody]SetGoalRequest setGoalRequest)
        {
            return base.SetGoal(setGoalRequest);
        }

        [Route("confirmation/resend")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> SendConfirmEmail([FromBody]string email)
        {
            return await base.SendConfirmEmail(email);
        }

        [Route("password/recovery")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult RecoverPassword([FromBody]RecoverPasswordRequest recoverPasswordRequest)
        {
            return base.RecoverPassword(recoverPasswordRequest);
        }

        [Route("goals/options")]
        [HttpGet]
        [AllowAnonymous]
        public new IActionResult GoalOptions()
        {
            return base.GoalOptions();
        }

        [Route("apikeys")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult GenerateApiAccess()
        {
            return base.GenerateApiAccess();
        }
    }
}