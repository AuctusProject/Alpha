using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Account
{
    public class GoalOptionBusiness : BaseBusiness<GoalOption, GoalOptionData>
    {
        public GoalOptionBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public List<GoalOption> List()
        {
            string cacheKey = "GoalOptions";
            List<GoalOption> options = MemoryCache.Get<List<GoalOption>>(cacheKey);
            if (options == null)
            {
                options = ListAll().OrderBy(c => c.Position).ToList();
                if (options != null)
                    MemoryCache.Set<List<GoalOption>>(cacheKey, options, 1440);
            }
            return options;
        }
    }
}
