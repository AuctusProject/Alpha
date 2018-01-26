using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Account
{
    public class ApiAccessBusiness : BaseBusiness<ApiAccess, ApiAccessData>
    {
        public ApiAccessBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public ApiAccess Create(string email)
        {
            var apiAccess = BaseCreation(email);
            apiAccess.ApiKey = Guid.NewGuid().ToString();
            Data.Insert(apiAccess);
            return apiAccess;
        }

        public ApiAccess GetLast(string email)
        {
            return List(email).OrderBy(c => c.CreationDate).LastOrDefault();
        }

        public void Delete(string email)
        {
            var apiAccess = BaseCreation(email);
            apiAccess.ApiKey = null;
            Data.Insert(apiAccess);
        }

        public List<ApiAccess> List(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            return Data.List(user.Id);
        }

        private ApiAccess BaseCreation(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var apiAccess = new ApiAccess();
            apiAccess.CreationDate = DateTime.UtcNow;
            apiAccess.UserId = user.Id;
            return apiAccess;
        }
    }
}
