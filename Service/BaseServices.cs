using Auctus.Business.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Service
{
    public abstract class BaseServices
    {
        protected readonly Cache MemoryCache;
        protected readonly ILoggerFactory Logger;

        protected BaseServices(ILoggerFactory loggerFactory, Cache cache)
        {
            MemoryCache = cache;
            Logger = loggerFactory;
        }

        protected UserBusiness UserBusiness { get { return new UserBusiness(Logger, MemoryCache); } }
        protected GoalOptionsBusiness GoalOptionsBusiness { get { return new GoalOptionsBusiness(Logger, MemoryCache); } }
        protected GoalBusiness GoalBusiness { get { return new GoalBusiness(Logger, MemoryCache); } }
        protected PasswordRecoveryBusiness PasswordRecoveryBusiness { get { return new PasswordRecoveryBusiness(Logger, MemoryCache); } }
    }
}
