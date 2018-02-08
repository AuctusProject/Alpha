using Auctus.DataAccess.Account;
using Auctus.DataAccess.Core;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Dapper;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Auctus.Business.Account
{
    public class UserBusiness : BaseBusiness<User, UserData>
    {
        public UserBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public User Login(string address, string emailOrUsername, string password)
        {
            BaseAddressValidation(address);
            BasePasswordValidation(password);
            BaseEmailOrUsernameValidation(emailOrUsername);

            var user = Data.GetByEmailOrUsername(emailOrUsername);
            if(user.Wallet.Address.ToUpper() != address.ToUpper())
                throw new ArgumentException("Wallet is invalid.");
            else if (user.Password != Security.Hash(password))
                throw new ArgumentException("Password is invalid.");
            else if (user.ConfirmationDate.HasValue)
                MemoryCache.Set<User>(user.Email, user);

            return user;
        }

        public async Task<User> SimpleRegister(string address, string username, string email, string password)
        {
            User user;
            using (var transaction = new TransactionalDapperCommand())
            {
                user = SetBaseUserCreation(username, email, password);
                transaction.Insert(user);
                var wallet = SetWalletCreation(user.Id, address);
                user.Wallet = wallet;
                transaction.Insert(wallet);
                transaction.Commit();
            }

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
        
        public void ConfirmEmail(string code)
        {
            var user = Data.GetByConfirmationCode(code);
            if (user == null)
                throw new ArgumentException("Invalid confirmation code.");

            user.ConfirmationDate = DateTime.UtcNow;
            Data.Update(user);
        }

        public bool IsValidEmailToRegister(string email)
        {
            User user = Data.GetByEmail(email);
            return user == null;
        }

        public bool IsValidUsernameToRegister(string username)
        {
            User user = Data.GetByUsername(username);
            return user == null;
        }

        public void ChangePassword(string email, string currentPassword, string newPassword)
        {
            var user = GetValidUser(email);
            if (user.Password != Security.Hash(currentPassword))
                throw new ArgumentException("Current password is incorrect.");
            UpdatePassword(user, newPassword);
        }

        public void UpdatePassword(User user, string password)
        {
            BasePasswordValidation(password);
            PasswordValidation(password);

            user.Password = Security.Hash(password);
            Data.Update(user);
        }
        
        public User GetValidUser(string email, string address = null)
        {
            BaseEmailValidation(email);
            var cacheKey = email.ToLower().Trim();
            var user = MemoryCache.Get<User>(cacheKey);
            if (user == null)
            {
                EmailValidation(email);
                user = Data.Get(email);
                if (user == null)
                    throw new ArgumentException("User cannot be found.");

                ValidateUserAddress(user, address);

                if (user.ConfirmationDate.HasValue)
                    MemoryCache.Set<User>(cacheKey, user);
                return user;
            }
            else
            {
                ValidateUserAddress(user, address);
                return user;
            }
        }

        private void ValidateUserAddress(User user, string address)
        {
            if (address != null && user.Wallet.Address.ToLower() != address.ToLower())
                throw new ArgumentException("Invalid user wallet.");
        }
        
        public User Get(Guid guid)
        {
            return Data.Get(guid);
        }

        public User Get(int id)
        {
            return Data.Get(id);
        }

        private async Task SendEmailConfirmation(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Verify your email on Auctus Alpha",
                string.Format("Thank you for support to Auctus Alpha. <br/><br/>To verify your account <a href='{0}/confirm?c={1}' target='_blank'>click here</a><br/><br/><small>If you do not recognize this email, just ignore the message.</small>", Config.WEB_URL, code));
        }

        private User SetBaseUserCreation(string username, string email, string password)
        {
            BaseEmailValidation(email);
            EmailValidation(email);
            BasePasswordValidation(password);
            PasswordValidation(password);

            User user = Data.Get(email);
            if (user != null)
                throw new ArgumentException("Email already registered.");

            user = Data.GetByUsername(username);
            if (user != null)
                throw new ArgumentException("Username already registered.");

            user = new User();
            user.Email = email.ToLower().Trim();
            user.Username = username.Trim();
            user.CreationDate = DateTime.UtcNow;
            user.Password = Security.Hash(password);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            return user;
        }

        private Wallet SetWalletCreation(int userId, string address)
        {
            User user = Data.GetByWalletAddress(address);
            if (user != null)
                throw new ArgumentException("Address already registered.");

            Wallet wallet = new Wallet();
            wallet.UserId = userId;
            wallet.Address = address;
            return wallet;
        }

        private void BaseEmailOrUsernameValidation(string emailOrUsername)
        {
            if (string.IsNullOrEmpty(emailOrUsername))
                throw new ArgumentException("Email or username must be filled.");
        }


        private void BaseEmailValidation(string email)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentException("Email must be filled.");
        }

        private void EmailValidation(string email)
        {
            if (!Email.IsValidEmail(email))
                throw new ArgumentException("Email informed is invalid.");
        }

        private void BasePasswordValidation(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password must be filled.");
        }
        private void BaseAddressValidation(string address)
        {
            if (string.IsNullOrEmpty(address))
                throw new ArgumentException("Address must be filled.");
        }

        private void PasswordValidation(string password)
        {
            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            if (password.Length > 100)
                throw new ArgumentException("Password cannot have more than 100 characters.");
            if (password.Contains(" "))
                throw new ArgumentException("Password cannot have spaces.");
        }
    }
}
