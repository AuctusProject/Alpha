﻿using Auctus.Business.Account;
using Auctus.Business.Advice;
using Auctus.Business.Asset;
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
        protected AdvisorBusiness AdvisorBusiness { get { return new AdvisorBusiness(Logger, MemoryCache); } }
        protected AdvisorDetailsBusiness AdvisorDetailsBusiness { get { return new AdvisorDetailsBusiness(Logger, MemoryCache); } }
        protected BuyBusiness BuyBusiness { get { return new BuyBusiness(Logger, MemoryCache); } }
        protected DistributionBusiness DistributionBusiness { get { return new DistributionBusiness(Logger, MemoryCache); } }
        protected PortfolioBusiness PortfolioBusiness { get { return new PortfolioBusiness(Logger, MemoryCache); } }
        protected PortfolioHistoryBusiness PortfolioHistoryBusiness { get { return new PortfolioHistoryBusiness(Logger, MemoryCache); } }
        protected ProjectionBusiness ProjectionBusiness { get { return new ProjectionBusiness(Logger, MemoryCache); } }
        protected AssetBusiness AssetBusiness { get { return new AssetBusiness(Logger, MemoryCache); } }
        protected AssetValueBusiness AssetValueBusiness { get { return new AssetValueBusiness(Logger, MemoryCache); } }
    }
}