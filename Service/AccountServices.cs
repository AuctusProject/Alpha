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
            UserBusiness.RecoverPassword(email, code, password);
        }

        public void ConfirmEmail(string email, string code)
        {
            UserBusiness.ConfirmEmail(email, code);
        }

        public async Task SendEmailForForgottenPassword(string email)
        {
            await UserBusiness.SendEmailForForgottenPassword(email);
        }

        public async Task ResendEmailConfirmation(string email)
        {
            await UserBusiness.ResendEmailConfirmation(email);
        }

        public async Task<User> Register(string email, string password)
        {
            return await UserBusiness.Register(email, password);
        }

        public User Login(string email, string password)
        {
            return UserBusiness.Login(email, password);
        }
    }
}
