using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
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
    [EnableCors("Default")]
    public class AccountV1Controller : AccountBaseController
    {
        public AccountV1Controller(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

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
        
        [Route("password/forgotten")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new async Task<IActionResult> ForgotPassword([FromBody]EmailRequest forgotPasswordRequest)
        {
            return await base.ForgotPassword(forgotPasswordRequest);
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
        public new IActionResult ChangePassword([FromBody]ChangePasswordRequest changePasswordRequest)
        {
            return base.ChangePassword(changePasswordRequest);
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
        public new async Task<IActionResult> SendConfirmEmail([FromBody]EmailRequest sendConfirmEmailRequest)
        {
            return await base.SendConfirmEmail(sendConfirmEmailRequest);
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

        [Route("apikeys/last")]
        [HttpGet]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult GetLastApiAccess()
        {
            return base.GetLastApiAccess();
        }

        [Route("apikeys")]
        [HttpDelete]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult DeleteApiAccess()
        {
            return base.DeleteApiAccess();
        }

        [Route("emails/{email}")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult IsValidEmailToRegister([FromRoute]string email)
        {
            return base.IsValidEmailToRegister(email);
        }

        [Route("usernames/{username}")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult IsValidUsernameToRegister([FromRoute]string username)
        {
            return base.IsValidUsernameToRegister(username);
        }

        [Route("addresses/{address}")]
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult IsValidAddressToRegister([FromRoute]string address)
        {
            return base.IsValidAddressToRegister(address);
        }

        [Route("faucet")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult Faucet([FromBody]FaucetRequest faucetRequest)
        {
            return base.Faucet(faucetRequest);
        }

        [Route("exchangeApi")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public new IActionResult SaveExchangeApiAccess([FromBody]ExchangeApiAccessRequest exchangeApiAccessRequest)
        {
            return base.SaveExchangeApiAccess(exchangeApiAccessRequest);
        }
    }
}
