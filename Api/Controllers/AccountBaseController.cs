using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Model.Account;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Web3;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    public class AccountBaseController : BaseController
    {
        protected AccountBaseController(ILoggerFactory loggerFactory, Cache cache, IServiceProvider serviceProvider, INodeServices nodeServices) : base(loggerFactory, cache, serviceProvider, nodeServices) { }

        protected virtual IActionResult Login(LoginRequest loginRequest)
        {
            if (loginRequest == null)
                return BadRequest(new { logged = false });

            Login login;
            try
            {
                login = AccountServices.Login(loginRequest.Address, loginRequest.EmailOrUsername, loginRequest.Password);
            }
            catch (ArgumentException)
            {
                return BadRequest(new { logged = false, error = "Credentials are invalid." });
            }
            if (login.PendingConfirmation)
            {
                return Ok(new { logged = false, error = "Pending email confirmation.", data = login });
            }
            return Ok(new { logged = true, jwt = GenerateToken(login.Email.ToLower().Trim()), data = login });
        }

        protected virtual async Task<IActionResult> SimpleRegister(SimpleRegisterRequest registerRequest)
        {
            if (registerRequest == null)
                return BadRequest();

            Login login;
            try
            {
                login = await AccountServices.SimpleRegister(registerRequest.Address, registerRequest.Username, registerRequest.Email, registerRequest.Password);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { jwt = GenerateToken(registerRequest.Email.ToLower().Trim()), data = login });
        }
        
        protected virtual async Task<IActionResult> ForgotPassword(EmailRequest forgotPasswordRequest)
        {
            if (forgotPasswordRequest == null || forgotPasswordRequest.Email == null)
                return BadRequest();

            try
            {
                await AccountServices.SendEmailForForgottenPassword(forgotPasswordRequest.Email);
            }
            catch (ArgumentException)
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
                var login = AccountServices.ConfirmEmail(confirmEmailRequest.Code);
                return Ok(new { jwt = GenerateToken(login.Email), data = login });
            }
            catch (ArgumentException)
            {
                return BadRequest(new { error = "Email could not be confirmed." });
            }
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
                AccountServices.CreateGoal(GetUser(), setGoalRequest.GoalOptionId, setGoalRequest.Timeframe, setGoalRequest.Risk,
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
            catch (ArgumentException)
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

        protected virtual IActionResult IsValidEmailToRegister(string email)
        {
            if (string.IsNullOrEmpty(email))
                return BadRequest();

            bool isValid;
            try
            {
                isValid = AccountServices.IsValidEmailToRegister(email);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { isValid = isValid });
        }

        protected virtual IActionResult IsValidUsernameToRegister(string username)
        {
            if (string.IsNullOrEmpty(username))
                return BadRequest();


            bool isValid;
            try
            {
                isValid = AccountServices.IsValidUsernameToRegister(username);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { isValid = isValid });
        }

        protected virtual IActionResult IsValidAddressToRegister(string address)
        {
            if (string.IsNullOrEmpty(address))
                return BadRequest();


            bool isValid;
            try
            {
                isValid = AccountServices.IsValidAddressToRegister(address);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            return Ok(new { isValid = isValid });
        }

        protected virtual IActionResult Faucet(FaucetRequest faucetRequest)
        {
            if (faucetRequest == null || string.IsNullOrWhiteSpace(faucetRequest.Address))
                return BadRequest();

            //var address = faucetRequest.Address.ToLower().Trim();
            //if (MemoryCache.Get<string>(address) != null)
            //    return new StatusCodeResult(429);
            try
            {
                var transactionHash = AccountServices.Faucet(faucetRequest.Address);
                //MemoryCache.Set<string>(address, address, 15);
                return Ok(new { transaction = transactionHash });
            }
            catch (Web3Exception ex)
            {
                if (ex.Code == 429)
                    return new StatusCodeResult(429);
                else
                    return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        protected virtual IActionResult GetUserBalance()
        {
            UserBalance userBalance = AccountServices.GetUserBalance(GetUser());
            return Ok(new { balance = userBalance });
        }
    }
}