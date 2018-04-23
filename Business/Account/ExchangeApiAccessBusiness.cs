
using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Auctus.DomainObjects.Portfolio;
using Auctus.DataAccess.Exchanges;

namespace Auctus.Business.Account
{
    public class ExchangeApiAccessBusiness : BaseBusiness<ExchangeApiAccess, ExchangeApiAccessData>
    {
        public ExchangeApiAccessBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public ExchangeApiAccess Create(string email, int exchangeId, string apiKey, string apiSecretKey)
        {
            ValidatePermissions(exchangeId, apiKey, apiSecretKey);
            var apiAccess = BaseCreation(email);
            apiAccess.ExchangeId = exchangeId;
            apiAccess.ApiKey = apiKey;
            apiAccess.ApiSecretKey = apiSecretKey;
            Data.Insert(apiAccess);
            return apiAccess;
        }

        private void ValidatePermissions(int exchangeId, string apiKey, string apiSecretKey)
        {
            var exchangeApi = ExchangeApi.GetById(exchangeId, apiKey, apiSecretKey);
            exchangeApi.ValidateAccessPermissions();
        }

        private ExchangeApiAccess BaseCreation(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            var apiAccess = new ExchangeApiAccess();
            apiAccess.CreationDate = DateTime.UtcNow;
            apiAccess.UserId = user.Id;
            return apiAccess;
        }

        public void Delete(int id)
        {
            var apiAccess = new ExchangeApiAccess();
            apiAccess.Id = id;
            Data.Delete(apiAccess);
        }

        public List<ExchangeApiAccess> List(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            return Data.List(user.Id);
        }

        public ExchangeApiAccess Get(int userId, int exchangeId)
        {
            return Data.Get(userId, exchangeId);
        }

        internal List<Model.Portfolio.Distribution> ConvertExchangeBalancesToAssetDistribution(List<ExchangeBalance> exchangeBalances)
        {
            var allAssets = AssetBusiness.ListAssets();
            //var totalAmount = exchangeBalances.Sum(c => c.UsdAmount);
            var distribution = new List<Model.Portfolio.Distribution>();
            foreach(var exchangeBalance in exchangeBalances)
            {
                var asset = allAssets.FirstOrDefault(c => c.Code.Trim().ToUpper() == exchangeBalance.CurrencyCode.Trim().ToUpper());
                if (asset != null)
                {
                    distribution.Add(new Model.Portfolio.Distribution()
                    {
                        Id = asset.Id,
                        Code = asset.Code,
                        Name = asset.Name,
                        //Percentage = exchangeBalance.Amount / totalAmount,
                        Type = asset.Type
                    });
                }
            }
            return distribution;
        }
    }
}
