using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Account
{
    public class ApiAccessBusiness : BaseBusiness<ApiAccess, ApiAccessData>
    {
        public ApiAccessBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public ApiAccess Create(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var apiAccess = new ApiAccess();
            apiAccess.CreationDate = DateTime.UtcNow;
            apiAccess.ApiKey = Guid.NewGuid().ToString();
            apiAccess.UserId = user.Id;
            Data.Insert(apiAccess);
            return apiAccess;
        }
    }
}
