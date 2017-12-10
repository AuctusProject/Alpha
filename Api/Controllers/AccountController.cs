using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.Authorization;
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { logged = false, error = ex.Message });
            }
            if (!user.ConfirmedEmail.HasValue)
            {
                return Ok(new { logged = false, message = "Pending email confirmation." });
            }
            return Ok(new { logged = true, jwt = GenerateToken(loginRequest.Email.ToLower().Trim()) });
        }
        
        [Route("register")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody]RegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();
           
            try
            {
                await AccountServices.Register(registerRequest.Email, registerRequest.Password);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { jwt = GenerateToken(registerRequest.Email.ToLower().Trim()) });
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        [Route("confirm")]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ConfirmEmail([FromBody]ConfirmEmailRequest confirmEmailRequest)
        {
            if (confirmEmailRequest == null)
                return BadRequest();

            try
            {
                AccountServices.ConfirmEmail(confirmEmailRequest.Email, confirmEmailRequest.Code);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
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
            catch (InvalidOperationException ex)
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
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
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }
    }
}
