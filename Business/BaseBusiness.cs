using Auctus.Business.Account;
using Auctus.Business.Advice;
using Auctus.Business.Asset;
using Auctus.DataAccess;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business
{
    public abstract class BaseBusiness<T, D> where D : BaseData<T>, new()
    {
        protected readonly ILoggerFactory LoggerFactory;
        protected readonly ILogger Logger;
        protected readonly Cache MemoryCache;
        
        protected D Data => new D();

        private UserBusiness _userBusiness;
        private PasswordRecoveryBusiness _passwordRecoveryBusiness;
        private GoalOptionsBusiness _goalOptionsBusiness;
        private GoalBusiness _goalBusiness;
        private AdvisorBusiness _advisorBusiness;
        private AdvisorDetailsBusiness _advisorDetailsBusiness;
        private BuyBusiness _buyBusiness;
        private DistributionBusiness _distributionBusiness;
        private PortfolioBusiness _portfolioBusiness;
        private PortfolioHistoryBusiness _portfolioHistoryBusiness;
        private ProjectionBusiness _projectionBusiness;
        private AssetBusiness _assetBusiness;
        private AssetValueBusiness _assetValueBusiness;

        protected BaseBusiness(ILoggerFactory loggerFactory, Cache cache)
        {
            MemoryCache = cache;
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
                    _userBusiness = new UserBusiness(LoggerFactory, MemoryCache);
                return _userBusiness;
            }
        }

        protected PasswordRecoveryBusiness PasswordRecoveryBusiness
        {
            get
            {
                if (_passwordRecoveryBusiness == null)
                    _passwordRecoveryBusiness = new PasswordRecoveryBusiness(LoggerFactory, MemoryCache);
                return _passwordRecoveryBusiness;
            }
        }

        protected GoalOptionsBusiness GoalOptionsBusiness
        {
            get
            {
                if (_goalOptionsBusiness == null)
                    _goalOptionsBusiness = new GoalOptionsBusiness(LoggerFactory, MemoryCache);
                return _goalOptionsBusiness;
            }
        }

        protected GoalBusiness GoalBusiness
        {
            get
            {
                if (_goalBusiness == null)
                    _goalBusiness = new GoalBusiness(LoggerFactory, MemoryCache);
                return _goalBusiness;
            }
        }

        protected AdvisorBusiness AdvisorBusiness
        {
            get
            {
                if (_advisorBusiness == null)
                    _advisorBusiness = new AdvisorBusiness(LoggerFactory, MemoryCache);
                return _advisorBusiness;
            }
        }

        protected AdvisorDetailsBusiness AdvisorDetailsBusiness
        {
            get
            {
                if (_advisorDetailsBusiness == null)
                    _advisorDetailsBusiness = new AdvisorDetailsBusiness(LoggerFactory, MemoryCache);
                return _advisorDetailsBusiness;
            }
        }

        protected BuyBusiness BuyBusiness
        {
            get
            {
                if (_buyBusiness == null)
                    _buyBusiness = new BuyBusiness(LoggerFactory, MemoryCache);
                return _buyBusiness;
            }
        }

        protected DistributionBusiness DistributionBusiness
        {
            get
            {
                if (_distributionBusiness == null)
                    _distributionBusiness = new DistributionBusiness(LoggerFactory, MemoryCache);
                return _distributionBusiness;
            }
        }

        protected PortfolioBusiness PortfolioBusiness
        {
            get
            {
                if (_portfolioBusiness == null)
                    _portfolioBusiness = new PortfolioBusiness(LoggerFactory, MemoryCache);
                return _portfolioBusiness;
            }
        }

        protected PortfolioHistoryBusiness PortfolioHistoryBusiness
        {
            get
            {
                if (_portfolioHistoryBusiness == null)
                    _portfolioHistoryBusiness = new PortfolioHistoryBusiness(LoggerFactory, MemoryCache);
                return _portfolioHistoryBusiness;
            }
        }

        protected ProjectionBusiness ProjectionBusiness
        {
            get
            {
                if (_projectionBusiness == null)
                    _projectionBusiness = new ProjectionBusiness(LoggerFactory, MemoryCache);
                return _projectionBusiness;
            }
        }

        protected AssetBusiness AssetBusiness
        {
            get
            {
                if (_assetBusiness == null)
                    _assetBusiness = new AssetBusiness(LoggerFactory, MemoryCache);
                return _assetBusiness;
            }
        }

        protected AssetValueBusiness AssetValueBusiness
        {
            get
            {
                if (_assetValueBusiness == null)
                    _assetValueBusiness = new AssetValueBusiness(LoggerFactory, MemoryCache);
                return _assetValueBusiness;
            }
        }
    }
}
