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
    public class GoalOptionsBusiness : BaseBusiness<GoalOptions, GoalOptionsData>
    {
        public GoalOptionsBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public List<GoalOptions> List()
        {
            string cacheKey = "GoalOptions";
            List<GoalOptions> options = MemoryCache.Get<List<GoalOptions>>(cacheKey);
            if (options == null)
            {
                options = ListAll().OrderBy(c => c.Position).ToList();
                if (options != null)
                    MemoryCache.Set<List<GoalOptions>>(cacheKey, options, 1440);
            }
            return options;
        }
    }
}
