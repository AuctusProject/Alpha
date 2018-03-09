using Auctus.Business.Account;
using Auctus.Business.Advisor;
using Auctus.Business.Portfolio;
using Auctus.Business.Asset;
using Auctus.DataAccess;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.NodeServices;

namespace Auctus.Business
{
    public abstract class BaseBusiness<T, D> where D : BaseData<T>, new()
    {
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly ILogger Logger;
        protected readonly Cache MemoryCache;
        protected readonly INodeServices NodeServices;

        protected D Data => new D();

        private UserBusiness _userBusiness;
        private PasswordRecoveryBusiness _passwordRecoveryBusiness;
        private GoalOptionBusiness _goalOptionsBusiness;
        private GoalBusiness _goalBusiness;
        private DepositBusiness _depositBusiness;
        private AdvisorBusiness _advisorBusiness;
        private AdvisorDetailBusiness _advisorDetailBusiness;
        private BuyBusiness _buyBusiness;
        private DistributionBusiness _distributionBusiness;
        private PortfolioBusiness _portfolioBusiness;
        private PortfolioHistoryBusiness _portfolioHistoryBusiness;
        private ProjectionBusiness _projectionBusiness;
        private AssetBusiness _assetBusiness;
        private AssetValueBusiness _assetValueBusiness;
        private ApiAccessBusiness _apiAccessBusiness;
        private WalletBusiness _walletBusiness;
        private TransactionBusiness _transactionBusiness;
        private BuyTransactionBusiness _buyTransactionBusiness;
        private PortfolioDetailBusiness _portfolioDetailBusiness;
        private EscrowResultTransactionBusiness _escrowResultTransactionBusiness;
        private EscrowResultBusiness _escrowResultBusiness;

        protected BaseBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices)
        {
            MemoryCache = cache;
            NodeServices = nodeServices;
            LoggerFactory = loggerFactory;
            Logger = loggerFactory.CreateLogger(GetType().Namespace);
        }

        public IEnumerable<T> ListAll()
        {
            return Data.SelectAll();
        }

        public void Insert(T obj)
        {
            Data.Insert(obj);
        }

        public void Update(T obj)
        {
            Data.Update(obj);
        }

        public void Delete(T obj)
        {
            Data.Delete(obj);
        }

        protected UserBusiness UserBusiness
        {
            get
            {
                if (_userBusiness == null)
                    _userBusiness = new UserBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _userBusiness;
            }
        }

        protected WalletBusiness WalletBusiness
        {
            get
            {
                if (_walletBusiness == null)
                    _walletBusiness = new WalletBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _walletBusiness;
            }
        }

        protected PasswordRecoveryBusiness PasswordRecoveryBusiness
        {
            get
            {
                if (_passwordRecoveryBusiness == null)
                    _passwordRecoveryBusiness = new PasswordRecoveryBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _passwordRecoveryBusiness;
            }
        }

        protected GoalOptionBusiness GoalOptionsBusiness
        {
            get
            {
                if (_goalOptionsBusiness == null)
                    _goalOptionsBusiness = new GoalOptionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _goalOptionsBusiness;
            }
        }

        protected GoalBusiness GoalBusiness
        {
            get
            {
                if (_goalBusiness == null)
                    _goalBusiness = new GoalBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _goalBusiness;
            }
        }

        protected DepositBusiness DepositBusiness
        {
            get
            {
                if (_depositBusiness == null)
                    _depositBusiness = new DepositBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _depositBusiness;
            }
        }

        protected AdvisorBusiness AdvisorBusiness
        {
            get
            {
                if (_advisorBusiness == null)
                    _advisorBusiness = new AdvisorBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _advisorBusiness;
            }
        }

        protected AdvisorDetailBusiness AdvisorDetailBusiness
        {
            get
            {
                if (_advisorDetailBusiness == null)
                    _advisorDetailBusiness = new AdvisorDetailBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _advisorDetailBusiness;
            }
        }

        protected BuyBusiness BuyBusiness
        {
            get
            {
                if (_buyBusiness == null)
                    _buyBusiness = new BuyBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _buyBusiness;
            }
        }

        protected DistributionBusiness DistributionBusiness
        {
            get
            {
                if (_distributionBusiness == null)
                    _distributionBusiness = new DistributionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _distributionBusiness;
            }
        }

        protected PortfolioBusiness PortfolioBusiness
        {
            get
            {
                if (_portfolioBusiness == null)
                    _portfolioBusiness = new PortfolioBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _portfolioBusiness;
            }
        }

        protected PortfolioHistoryBusiness PortfolioHistoryBusiness
        {
            get
            {
                if (_portfolioHistoryBusiness == null)
                    _portfolioHistoryBusiness = new PortfolioHistoryBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _portfolioHistoryBusiness;
            }
        }

        protected ProjectionBusiness ProjectionBusiness
        {
            get
            {
                if (_projectionBusiness == null)
                    _projectionBusiness = new ProjectionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _projectionBusiness;
            }
        }

        protected AssetBusiness AssetBusiness
        {
            get
            {
                if (_assetBusiness == null)
                    _assetBusiness = new AssetBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _assetBusiness;
            }
        }

        protected AssetValueBusiness AssetValueBusiness
        {
            get
            {
                if (_assetValueBusiness == null)
                    _assetValueBusiness = new AssetValueBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _assetValueBusiness;
            }
        }

        protected ApiAccessBusiness ApiAccessBusiness
        {
            get
            {
                if (_apiAccessBusiness == null)
                    _apiAccessBusiness = new ApiAccessBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _apiAccessBusiness;
            }
        }

        protected TransactionBusiness TransactionBusiness
        {
            get
            {
                if (_transactionBusiness == null)
                    _transactionBusiness = new TransactionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _transactionBusiness;
            }
        }

        protected BuyTransactionBusiness BuyTransactionBusiness
        {
            get
            {
                if (_buyTransactionBusiness == null)
                    _buyTransactionBusiness = new BuyTransactionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _buyTransactionBusiness;
            }
        }

        protected PortfolioDetailBusiness PortfolioDetailBusiness
        {
            get
            {
                if (_portfolioDetailBusiness == null)
                    _portfolioDetailBusiness = new PortfolioDetailBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _portfolioDetailBusiness;
            }
        }

        protected EscrowResultBusiness EscrowResultBusiness
        {
            get
            {
                if (_escrowResultBusiness == null)
                    _escrowResultBusiness = new EscrowResultBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _escrowResultBusiness;
            }
        }

        protected EscrowResultTransactionBusiness EscrowResultTransactionBusiness
        {
            get
            {
                if (_escrowResultTransactionBusiness == null)
                    _escrowResultTransactionBusiness = new EscrowResultTransactionBusiness(LoggerFactory, MemoryCache, NodeServices);
                return _escrowResultTransactionBusiness;
            }
        }
    }
}
