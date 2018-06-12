using Auctus.DomainObjects.Portfolio;
using Dapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Auctus.DataAccess.Portfolio
{
    public class PortfolioHistoryData : BaseMongo<PortfolioHistory>
    {
        public override string CollectionName => "PortfolioHistory";   

        public PortfolioHistory LastHistory(int portfolioId)
        {
            var value = Collection.Find(x => x.PortfolioId == portfolioId).SortByDescending(x => x.Date).FirstOrDefault();

            return value;
        }

        public List<PortfolioHistory> ListHistory(int portfolioId)
        {
            var values = Collection.Find(x => x.PortfolioId == portfolioId).SortByDescending(x => x.Date);

            return values.ToList();
        }
    }
}
