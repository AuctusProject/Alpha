using Auctus.DataAccess.Portfolio;
using Auctus.DomainObjects.Portfolio;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Portfolio
{
    public class PortfolioDetailBusiness : BaseBusiness<PortfolioDetail, PortfolioDetailData>
    {
        public PortfolioDetailBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public PortfolioDetail SetNew(int portfolioId, decimal? price, string name, string description, bool enabled)
        {
            if (price.HasValue && price < 1)
                throw new ArgumentException("Invalid price.");

            var portfolioDetail = new PortfolioDetail();
            portfolioDetail.PortfolioId = portfolioId;
            portfolioDetail.Date = DateTime.UtcNow;
            portfolioDetail.Name = name;
            portfolioDetail.Description = description;
            portfolioDetail.Enabled = enabled;
            portfolioDetail.Price = price;
            return portfolioDetail;
        }

        public PortfolioDetail Create(int portfolioId, decimal? price, string name, string description, bool enabled)
        {
            var portfolioDetail = SetNew(portfolioId, price, name, description, enabled);
            Data.Insert(portfolioDetail);
            return portfolioDetail;
        }
    }
}
