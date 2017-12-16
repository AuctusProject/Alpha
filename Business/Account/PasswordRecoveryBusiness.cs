using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Account
{
    public class PasswordRecoveryBusiness : BaseBusiness<PasswordRecovery, PasswordRecoveryData>
    {
        public PasswordRecoveryBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

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
        
        public void RecoverPassword(string email, string code, string password)
        {
            var user = UserBusiness.GetValidUser(email);
            var recovery = Data.Get(user.Id);
            if (recovery == null)
                throw new ArgumentException("There is no request for recover password.");
            if (code != recovery.Token)
                throw new ArgumentException("Invalid recover password code.");
            if (DateTime.UtcNow > recovery.Date.AddMinutes(60))
                throw new ArgumentException("Recover password code is expired.");

            UserBusiness.UpdatePassword(user, password);
        }
        
        private async Task SendForgottenPassword(string email, string code)
        {
            await Email.SendAsync(
                new string[] { email },
                "Recover your password from Auctus Alpha",
                string.Format("To recover your password from Auctus Alpha <a href='{0}/recoverpassword?c={1}' target='_blank'>click here</a><br/><br/><small>If you do not recognize this email, just ignore the message.</small>", Config.WEB_URL, code));
        }
    }
}
