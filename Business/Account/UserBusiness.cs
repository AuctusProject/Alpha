using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Dapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Account
{
    public class UserBusiness : BaseBusiness<User, UserData>
    {
        public UserBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public User Login(string email, string password)
        {
            BaseEmailValidation(email);
            BasePasswordValidation(password);

            var user = GetUser(email);
            if (user == null || user.Password != Security.Encrypt(password))
                throw new InvalidOperationException("Credentials are invalid.");

            return user;
        }

        public async Task<User> Register(string email, string password)
        {
            BaseEmailValidation(email);
            BasePasswordValidation(password);
            PasswordValidation(password);

            User user = GetUser(email);
            if (user != null)
                throw new InvalidOperationException("Email already registered.");

            user = new User();
            user.Email = email.ToLower().Trim();
            user.Creation = DateTime.UtcNow;
            user.Password = Security.Encrypt(password);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            Data.Insert(user);

            await SendEmailConfirmation(user.Email, user.ConfirmationCode);

            return user;
        }

        public async Task ResendEmailConfirmation(string email)
        {
            var user = GetValidUser(email);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            Data.Update(user);

            await SendEmailConfirmation(email, user.ConfirmationCode);
        }

        public async Task SendEmailForForgottenPassword(string email)
        {
            var user = GetValidUser(email);
            user.RecoverPasswordCode = Guid.NewGuid().ToString();
            user.RecoverPasswordDate = DateTime.UtcNow;
            Data.Update(user);

            await SendForgottenPassword(email, user.RecoverPasswordCode);
        }

        public void ConfirmEmail(string email, string code)
        {
            var user = GetValidUser(email);
            if (code != user.ConfirmationCode)
                throw new InvalidOperationException("Invalid confirmation code.");

            user.ConfirmedEmail = DateTime.UtcNow;
            Data.Update(user);
        }

        public void RecoverPassword(string email, string code, string password)
        {
            var user = GetValidUser(email);
            if (code != user.RecoverPasswordCode)
                throw new InvalidOperationException("Invalid recover password code.");
            if (!user.RecoverPasswordDate.HasValue || DateTime.UtcNow > user.RecoverPasswordDate.Value.AddDays(1))
                throw new InvalidOperationException("Recover password code is expired.");

            BasePasswordValidation(password);
            PasswordValidation(password);

            user.Password = Security.Encrypt(password);
            Data.Update(user);
        }

        public void ChangePassword(string email, string password)
        {
            var user = GetValidUser(email);
            BasePasswordValidation(password);
            PasswordValidation(password);

            user.Password = Security.Encrypt(password);
            Data.Update(user);
        }

        private async Task SendForgottenPassword(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Recover your password from Auctus Alpha",
                string.Format("To recover your password from Auctus Alpha <a href='{0}/recoverpassword?c={1}' target='_blank'>click here</a><br/><br/><small>If you do not recognize this email, just ignore the message.</small>", Config.WEB_URL, code));
        }

        private async Task SendEmailConfirmation(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Verify your email on Auctus Alpha",
                string.Format("Thank you for support to Auctus Alpha. <br/><br/>To verify your account <a href='{0}/confirm?c={1}' target='_blank'>click here</a><br/><br/><small>If you do not recognize this email, just ignore the message.</small>", Config.WEB_URL, code));
        }

        private User GetUser(string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Email", email.ToLower().Trim(), DbType.AnsiString);
            return Data.SelectByParameters<User>(parameters).SingleOrDefault();
        }

        private User GetValidUser(string email)
        {
            BaseEmailValidation(email);
            var user = GetUser(email);
            if (user == null)
                throw new InvalidOperationException("User cannot be found.");
            return user;
        }

        private void BaseEmailValidation(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new InvalidOperationException("Email must be filled.");
            if (!Email.IsValidEmail(email))
                throw new InvalidOperationException("Email informed is invalid.");
        }

        private void BasePasswordValidation(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new InvalidOperationException("Password must be filled.");
        }

        private void PasswordValidation(string password)
        {
            if (password.Length < 8)
                throw new InvalidOperationException("Password must be at least 8 characters.");
            if (password.Length > 30)
                throw new InvalidOperationException("Password cannot have more than 30 characters.");
            if (password.Contains(" "))
                throw new InvalidOperationException("Password cannot have spaces.");
        }
    }
}
