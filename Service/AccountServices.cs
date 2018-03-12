using Auctus.DomainObjects.Account;
using Auctus.Model;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class AccountServices : BaseServices
    {
        public AccountServices(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public void ChangePassword(string email, string currentPassword, string newPassword)
        {
            UserBusiness.ChangePassword(email, currentPassword, newPassword);
        }

        public void RecoverPassword(string code, string password)
        {
            PasswordRecoveryBusiness.RecoverPassword(code, password);
        }

        public Login ConfirmEmail(string code)
        {
            return UserBusiness.ConfirmEmail(code);
        }

        public async Task SendEmailForForgottenPassword(string email)
        {
            await PasswordRecoveryBusiness.SendEmailForForgottenPassword(email);
        }

        public async Task ResendEmailConfirmation(string email)
        {
            await UserBusiness.ResendEmailConfirmation(email);
        }

        public async Task<Login> SimpleRegister(string address, string username, string email, string password)
        {
            return await UserBusiness.SimpleRegister(address, username, email, password);
        }

        //public async Task<User> FullRegister(string email, string password, int goalOptionId, int? timeframe, int risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        //{
        //    return await UserBusiness.FullRegister(email, password, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        //}

        public Goal CreateGoal(string email, int goalOptionId, int timeframe, int risk, double? targetAmount, double startingAmount, double monthlyContribution)
        {
            return GoalBusiness.Create(email, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public User GetUser(Guid guid)
        {
            return UserBusiness.Get(guid);
        }

        public ApiAccess CreateApiAccess(string email)
        {
            return ApiAccessBusiness.Create(email);
        }

        public ApiAccess GetLastApiAccess(string email)
        {
            return ApiAccessBusiness.GetLast(email);
        }

        public void DeleteApiAccess(string email)
        {
            ApiAccessBusiness.Delete(email);
        }

        public Login Login(string address, string emailOrUsername, string password)
        {
            return UserBusiness.Login(address, emailOrUsername, password);
        }

        public List<GoalOption> ListGoalsOptions()
        {
            return GoalOptionsBusiness.List();
        }

        public bool IsValidEmailToRegister(string email)
        {
            return UserBusiness.IsValidEmailToRegister(email);
        }

        public bool IsValidUsernameToRegister(string username)
        {
            return UserBusiness.IsValidUsernameToRegister(username);
        }

        public bool IsValidAddressToRegister(string address)
        {
            return UserBusiness.IsValidAddressToRegister(address);
        }

        public string Faucet(string address)
        {
            return WalletBusiness.Faucet(address);
        }

        public UserBalance GetUserBalance(string email)
        {
            return UserBusiness.GetUserBalance(email);
        }

        public List<User> ListUsersByPerformance()
        {
            return UserBusiness.ListUsersByPerformance();
        }

        public bool CheckTelegramParticipation(string phoneNumber)
        {
            return UserBusiness.CheckTelegramParticipation(phoneNumber);
        }
    }
}
