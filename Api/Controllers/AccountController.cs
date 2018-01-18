using Api.Model.Account;
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
    [Route("api/account/")]
    public class AccountController : BaseController
    {
        public AccountController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base (loggerFactory, cache, serviceProvider) { }

        [Route("login")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Login([FromBody]LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest(new { logged = false });

            User user;
            try
            {
                user = AccountServices.Login(loginRequest.Email, loginRequest.Password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { logged = false, error = "Credentials are invalid." });
            }
            if (!user.ConfirmationDate.HasValue)
            {
                return Ok(new { logged = false, message = "Pending email confirmation." });
            }
            return Ok(new { logged = true, jwt = GenerateToken(loginRequest.Email.ToLower().Trim()), email = user.Email });
        }
        
        [Route("simpleregister")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SimpleRegister([FromBody]SimpleRegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();

            User user;
            try
            {
                user = await AccountServices.SimpleRegister(registerRequest.Email, registerRequest.Password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { jwt = GenerateToken(registerRequest.Email.ToLower().Trim()), email = user.Email });
        }

        [Route("fullregister")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FullRegister([FromBody]FullRegisterRequest registerRequest)
        {
            if (registerRequest == null || registerRequest.User == null || registerRequest.Goal == null)
                return BadRequest();

            User user;
            try
            {
                user = await AccountServices.FullRegister(registerRequest.User.Email, 
                                                            registerRequest.User.Password, 
                                                            registerRequest.Goal.GoalOption.Id, 
                                                            registerRequest.Goal.Timeframe, 
                                                            registerRequest.Goal.Risk, 
                                                            registerRequest.Goal.TargetAmount, 
                                                            registerRequest.Goal.StartingAmount, 
                                                            registerRequest.Goal.MonthlyContribution);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { jwt = GenerateToken(registerRequest.User.Email.ToLower().Trim()), email = user.Email });
        }

        [Route("forgotpassword")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody]string email)
        {
            try
            {
                await AccountServices.SendEmailForForgottenPassword(email);
            }
            catch (ArgumentException ex)
            {
                //return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("confirm")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ConfirmEmail([FromBody]ConfirmEmailRequest confirmEmailRequest)
        {
            if (confirmEmailRequest.Code == null)
                return BadRequest();

            try
            {
                AccountServices.ConfirmEmail(confirmEmailRequest.Code);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Email could not be confirmed." });
            }
            return Ok();
        }

        [Route("changepassword")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ChangePassword([FromBody]string password)
        {
            try
            {
                AccountServices.ChangePassword(GetUser(), password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("goal")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SetGoal([FromBody]SetGoalRequest setGoalRequest)
        {
            if (setGoalRequest == null)
                return BadRequest();

            try
            {
                AccountServices.CreateGoal(GetUser(), setGoalRequest.GoalOptionId, setGoalRequest.Timeframe, setGoalRequest.Risk, 
                    setGoalRequest.TargetAmount, setGoalRequest.StartingAmount, setGoalRequest.MonthlyContribution);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("sendconfirm")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SendConfirmEmail([FromBody]string email)
        {
            try
            {
                await AccountServices.ResendEmailConfirmation(email);
            }
            catch (ArgumentException ex)
            {
                //return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("recoverpassword")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult RecoverPassword([FromBody]RecoverPasswordRequest recoverPasswordRequest)
        {
            if (recoverPasswordRequest == null)
                return BadRequest();

            try
            {
                AccountServices.RecoverPassword(recoverPasswordRequest.Email, recoverPasswordRequest.Code, recoverPasswordRequest.Password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = "Recover password could not be validated." });
            }
            return Ok();
        }

        [Route("goaloptions")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GoalOptions()
        {
            var options = AccountServices.ListGoalsOptions();
            return Ok(options.Select(c => new { id = c.Id, description = c.Description, risk = c.Risk}));
        }

        [Route("apikey")]
        [HttpPost]
        [Authorize("Bearer")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GenerateApiAccess()
        {
            ApiAccess apiAccess;
            try
            {
                apiAccess = AccountServices.CreateApiAccess(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { key = apiAccess.ApiKey });
        }
    }
}
