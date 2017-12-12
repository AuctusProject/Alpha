using Auctus.Business.Account;
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
    }
}
