﻿using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Account
{
    public class PasswordRecoveryBusiness : BaseBusiness<PasswordRecovery, PasswordRecoveryData>
    {
        public PasswordRecoveryBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public async Task SendEmailForForgottenPassword(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var recovery = Data.Get(user.Id);
            if (recovery == null)
            {
                recovery = new PasswordRecovery();
                recovery.UserId = user.Id;
                recovery.Date = DateTime.UtcNow;
                recovery.Token = Guid.NewGuid().ToString();
                Data.Insert(recovery);
            }
            else
            {
                recovery.Date = DateTime.UtcNow;
                recovery.Token = Guid.NewGuid().ToString();
                Data.Update(recovery);
            }
            
            await SendForgottenPassword(email, recovery.Token);
        }
        
        public void RecoverPassword(string code, string password)
        {
            var recovery = Data.Get(code);
            if (recovery == null)
                throw new ArgumentException("There is no request for recover password.");
            if (DateTime.UtcNow > recovery.Date.AddMinutes(60))
                throw new ArgumentException("Recover password code is expired.");
            
            UserBusiness.UpdatePassword(UserBusiness.Get(recovery.UserId), password);
        }
        
        private async Task SendForgottenPassword(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Reset your password - Auctus Alpha",
                string.Format(@"Hello,
<br/><br/>
You told us you forgot your password. If you really did, <a href='{0}/forgot-password-reset?c={1}' target='_blank'>click here</a> to choose a new one.
<br/><br/>
If you didn't mean to reset your password, then you can just ignore this email. Your password will not change.
<br/><br/>
Thanks,
<br/>
Auctus Team", Config.WEB_URL, code));
        }
    }
}
