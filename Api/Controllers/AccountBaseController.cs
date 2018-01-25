using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class AccountBaseController : BaseController
    {
        protected AccountBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider) : base(loggerFactory, cache, serviceProvider) { }

        protected virtual IActionResult Login(LoginRequest loginRequest)
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
                return Ok(new { logged = false, error = "Pending email confirmation." });
            }
            return Ok(new { logged = true, jwt = GenerateToken(loginRequest.Email.ToLower().Trim()), email = user.Email });
        }

        protected virtual async Task<IActionResult> SimpleRegister(SimpleRegisterRequest registerRequest)
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

        protected virtual async Task<IActionResult> FullRegister(FullRegisterRequest registerRequest)
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

        protected virtual async Task<IActionResult> ForgotPassword(EmailRequest forgotPasswordRequest)
        {
            if (forgotPasswordRequest == null || forgotPasswordRequest.Email == null)
                return BadRequest();

            try
            {
                await AccountServices.SendEmailForForgottenPassword(forgotPasswordRequest.Email);
            }
            catch (ArgumentException ex)
            {
                //return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult ConfirmEmail(ConfirmEmailRequest confirmEmailRequest)
        {
            if (confirmEmailRequest == null || confirmEmailRequest.Code == null)
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

        protected virtual IActionResult ChangePassword(ChangePasswordRequest changePasswordRequest)
        {
            if (changePasswordRequest == null || changePasswordRequest.CurrentPassword == null || changePasswordRequest.NewPassword == null)
                return BadRequest();

            try
            {
                AccountServices.ChangePassword(GetUser(), changePasswordRequest.CurrentPassword, changePasswordRequest.NewPassword);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult SetGoal(SetGoalRequest setGoalRequest)
        {
            if (setGoalRequest == null)
                return BadRequest();

            try
            {
                AccountServices.CreateGoal(GetUser(), setGoalRequest.GoalOption.Id, setGoalRequest.Timeframe, setGoalRequest.Risk,
                    setGoalRequest.TargetAmount, setGoalRequest.StartingAmount, setGoalRequest.MonthlyContribution);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual async Task<IActionResult> SendConfirmEmail(EmailRequest sendConfirmEmailRequest)
        {
            if (sendConfirmEmailRequest == null || sendConfirmEmailRequest.Email == null)
                return BadRequest();

            try
            {
                await AccountServices.ResendEmailConfirmation(sendConfirmEmailRequest.Email);
            }
            catch (ArgumentException ex)
            {
                //return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult RecoverPassword(RecoverPasswordRequest recoverPasswordRequest)
        {
            if (recoverPasswordRequest == null)
                return BadRequest();

            try
            {
                AccountServices.RecoverPassword(recoverPasswordRequest.Code, recoverPasswordRequest.Password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }

        protected virtual IActionResult GoalOptions()
        {
            var options = AccountServices.ListGoalsOptions();
            return Ok(options.Select(c => new { id = c.Id, description = c.Description, risk = c.Risk }));
        }

        protected virtual IActionResult GenerateApiAccess()
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

        protected virtual IActionResult GetLastApiAccess()
        {
            ApiAccess apiAccess;
            try
            {
                apiAccess = AccountServices.GetLastApiAccess(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { key = apiAccess?.ApiKey });
        }

        protected virtual IActionResult DeleteApiAccess()
        {
            try
            {
                AccountServices.DeleteApiAccess(GetUser());
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok();
        }
    }
}