
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
            ValidateCreation(email, exchangeId);
            ValidatePermissions(exchangeId, apiKey, apiSecretKey);
            var apiAccess = BaseCreation(email);
            apiAccess.ExchangeId = exchangeId;
            apiAccess.ApiKey = apiKey;
            apiAccess.ApiSecretKey = apiSecretKey;
            Data.Insert(apiAccess);
            return apiAccess;
        }

        private void ValidateCreation(string email, int exchangeId)
        {
            var user = UserBusiness.GetValidUser(email);
            var exchangeApiAccess = ExchangeApiAccessBusiness.Get(user.Id, exchangeId);
            if(exchangeApiAccess!=null)
                throw new ArgumentException("Exchange API access is already saved");
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
            var distribution = new List<Model.Portfolio.Distribution>();
            var currentValues = AssetCurrentValueBusiness.ListAll();

            foreach (var exchangeBalance in exchangeBalances)
            {
                exchangeBalance.Asset = allAssets.FirstOrDefault(c => c.Code.Trim().ToUpper() == exchangeBalance.CurrencyCode.Trim().ToUpper());
                exchangeBalance.CurrentUsdValue = exchangeBalance.Amount * currentValues.FirstOrDefault(c => c.AssetId == exchangeBalance.Asset.Id)?.Value;
            }

            var totalAmount = exchangeBalances.Sum(c => c.CurrentUsdValue);

            foreach (var exchangeBalance in exchangeBalances)
            {
                if (exchangeBalance.Asset != null)
                {
                    distribution.Add(new Model.Portfolio.Distribution()
                    {
                        Code = exchangeBalance.CurrencyCode.Trim().ToUpper(),
                        Id = exchangeBalance.Asset.Id,
                        Name = exchangeBalance.Asset.Name,
                        Percentage = Math.Round(((exchangeBalance.CurrentUsdValue / totalAmount) ?? 0) * 100D, 1),
                        Type = exchangeBalance.Asset.Type,
                    });
                }
            }
            return distribution;
        }
    }
}
