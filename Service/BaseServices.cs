using Auctus.Business.Account;
using Auctus.Business.Advisor;
using Auctus.Business.Asset;
using Auctus.Business.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
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
        protected readonly INodeServices NodeServices;

        protected BaseServices(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices)
        {
            MemoryCache = cache;
            Logger = loggerFactory;
            NodeServices = nodeServices;
        }

        protected UserBusiness UserBusiness { get { return new UserBusiness(Logger, MemoryCache, NodeServices); } }
        protected GoalOptionBusiness GoalOptionsBusiness { get { return new GoalOptionBusiness(Logger, MemoryCache, NodeServices); } }
        protected GoalBusiness GoalBusiness { get { return new GoalBusiness(Logger, MemoryCache, NodeServices); } }
        protected PasswordRecoveryBusiness PasswordRecoveryBusiness { get { return new PasswordRecoveryBusiness(Logger, MemoryCache, NodeServices); } }
        protected AdvisorBusiness AdvisorBusiness { get { return new AdvisorBusiness(Logger, MemoryCache, NodeServices); } }
        protected AdvisorDetailBusiness AdvisorDetailBusiness { get { return new AdvisorDetailBusiness(Logger, MemoryCache, NodeServices); } }
        protected BuyBusiness BuyBusiness { get { return new BuyBusiness(Logger, MemoryCache, NodeServices); } }
        protected FollowBusiness FollowBusiness { get { return new FollowBusiness(Logger, MemoryCache, NodeServices); } }
        protected DistributionBusiness DistributionBusiness { get { return new DistributionBusiness(Logger, MemoryCache, NodeServices); } }
        protected PortfolioBusiness PortfolioBusiness { get { return new PortfolioBusiness(Logger, MemoryCache, NodeServices); } }
        protected PortfolioHistoryBusiness PortfolioHistoryBusiness { get { return new PortfolioHistoryBusiness(Logger, MemoryCache, NodeServices); } }
        protected ProjectionBusiness ProjectionBusiness { get { return new ProjectionBusiness(Logger, MemoryCache, NodeServices); } }
        protected AssetBusiness AssetBusiness { get { return new AssetBusiness(Logger, MemoryCache, NodeServices); } }
        protected AssetValueBusiness AssetValueBusiness { get { return new AssetValueBusiness(Logger, MemoryCache, NodeServices); } }
        protected AssetCurrentValueBusiness AssetCurrentValueBusiness { get { return new AssetCurrentValueBusiness(Logger, MemoryCache, NodeServices); } }
        protected ApiAccessBusiness ApiAccessBusiness { get { return new ApiAccessBusiness(Logger, MemoryCache, NodeServices); } }
        protected ExchangeApiAccessBusiness ExchangeApiAccessBusiness { get { return new ExchangeApiAccessBusiness(Logger, MemoryCache, NodeServices); } }
        protected WalletBusiness WalletBusiness { get { return new WalletBusiness(Logger, MemoryCache, NodeServices); } }
        protected TransactionBusiness TransactionBusiness { get { return new TransactionBusiness(Logger, MemoryCache, NodeServices); } }
        protected BuyTransactionBusiness BuyTransactionBusiness { get { return new BuyTransactionBusiness(Logger, MemoryCache, NodeServices); } }
        protected PortfolioDetailBusiness PortfolioDetailBusiness { get { return new PortfolioDetailBusiness(Logger, MemoryCache, NodeServices); } }
        protected EscrowResultBusiness EscrowResultBusiness { get { return new EscrowResultBusiness(Logger, MemoryCache, NodeServices); } }
        protected EscrowResultTransactionBusiness EscrowResultTransactionBusiness { get { return new EscrowResultTransactionBusiness(Logger, MemoryCache, NodeServices); } }
    }
}
