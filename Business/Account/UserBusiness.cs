using Auctus.DataAccess.Account;
using Auctus.DataAccess.Core;
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
using System.Transactions;

namespace Auctus.Business.Account
{
    public class UserBusiness : BaseBusiness<User, UserData>
    {
        public UserBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public User Login(string email, string password)
        {
            BasePasswordValidation(password);
            BaseEmailValidation(email);
            EmailValidation(email);

            var user = Data.Get(email);
            if (user.Password != Security.Hash(password))
                throw new ArgumentException("Password is invalid.");
            else if (user.ConfirmationDate.HasValue)
                MemoryCache.Set<User>(user.Email, user);

            return user;
        }

        public async Task<User> SimpleRegister(string email, string password)
        {
            var user = SetBaseUserCreation(email, password);
            Data.Insert(user);

            await SendEmailConfirmation(user.Email, user.ConfirmationCode);

            return user;
        }

        public async Task<User> FullRegister(string email, string password, int goalOptionId, int? timeframe, int risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            var user = SetBaseUserCreation(email, password);
            using (var transaction = new TransactionalDapperCommand())
            {
                transaction.Insert(user, "[User]");
                var goal = GoalBusiness.SetNew(user.Id, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
                transaction.Insert(goal);
                var option = GoalOptionsBusiness.List().Single(c => c.Id == goal.GoalOptionId);
                var portfolio = PortfolioBusiness.GetByRisk(AdvisorBusiness.DefaultAdvisorId, RiskType.Get(goal.Risk, option.Risk));
                var buyAuctusAdvisor = BuyBusiness.SetNew(AdvisorBusiness.DefaultAdvisorId, portfolio.ProjectionId.Value, goal.Id, 999999);
                transaction.Insert(buyAuctusAdvisor);
                user.Goal = goal;
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
        
        public void ChangePassword(string email, string password)
        {
            UpdatePassword(GetValidUser(email), password);
        }

        public void UpdatePassword(User user, string password)
        {
            BasePasswordValidation(password);
            PasswordValidation(password);

            user.Password = Security.Hash(password);
            Data.Update(user);
        }
        
        public User GetValidUser(string email)
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
                else if (user.ConfirmationDate.HasValue)
                    MemoryCache.Set<User>(cacheKey, user);
            }
            return user;
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

        private User SetBaseUserCreation(string email, string password)
        {
            BaseEmailValidation(email);
            EmailValidation(email);
            BasePasswordValidation(password);
            PasswordValidation(password);

            User user = Data.Get(email);
            if (user != null)
                throw new ArgumentException("Email already registered.");

            user = new User();
            user.Email = email.ToLower().Trim();
            user.CreationDate = DateTime.UtcNow;
            user.Password = Security.Hash(password);
            user.ConfirmationCode = Guid.NewGuid().ToString();
            return user;
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

        private void PasswordValidation(string password)
        {
            if (password.Length < 8)
                throw new ArgumentException("Password must be at least 8 characters.");
            if (password.Length > 30)
                throw new ArgumentException("Password cannot have more than 30 characters.");
            if (password.Contains(" "))
                throw new ArgumentException("Password cannot have spaces.");
        }
    }
}
