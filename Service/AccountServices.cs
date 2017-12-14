using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Service
{
    public class AccountServices : BaseServices
    {
        public AccountServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public void ChangePassword(string email, string password)
        {
            UserBusiness.ChangePassword(email, password);
        }

        public void RecoverPassword(string email, string code, string password)
        {
            PasswordRecoveryBusiness.RecoverPassword(email, code, password);
        }

        public void ConfirmEmail(string email, string code)
        {
            UserBusiness.ConfirmEmail(email, code);
        }

        public async Task SendEmailForForgottenPassword(string email)
        {
            await PasswordRecoveryBusiness.SendEmailForForgottenPassword(email);
        }

        public async Task ResendEmailConfirmation(string email)
        {
            await UserBusiness.ResendEmailConfirmation(email);
        }

        public async Task<User> SimpleRegister(string email, string password)
        {
            return await UserBusiness.SimpleRegister(email, password);
        }

        public async Task<User> FullRegister(string email, string password, int goalOptionId, int? timeframe, int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            return await UserBusiness.FullRegister(email, password, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public Goal CreateGoal(string email, int goalOptionId, int? timeframe, int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            return GoalBusiness.Create(email, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public User Login(string email, string password)
        {
            return UserBusiness.Login(email, password);
        }

        public List<GoalOption> ListGoalsOptions()
        {
            return GoalOptionsBusiness.List();
        }
    }
}
