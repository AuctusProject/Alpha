using Auctus.DomainObjects.Portfolio;
using Dapper;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Portfolio
{
    public class DistributionData : BaseMongo<Distribution>
    {
        public override string CollectionName => "Distribution";

        private const string LIST_DISTRIBUTIONS_FROM_PORTFOLIO = 
            @"SELECT d.*, p.* FROM Distribution d 
                INNER JOIN Projection p ON p.Id = d.ProjectionId
                WHERE p.PortfolioId = @PortfolioId";
        
        public List<Distribution> List(IEnumerable<int> projectionsId)
        {
            var filterBuilder = Builders<Distribution>.Filter;
            var filter = filterBuilder.In(x => x.ProjectionId, projectionsId.ToArray());
            var value = Collection.Find(filter).ToList();

            return value;
        }

        public List<Distribution> ListFromPortfolio(IEnumerable<int> portfoliosIds)
        {
            var filterBuilder = Builders<Distribution>.Filter;
            var filter = filterBuilder.In(x => x.PortfolioId, portfoliosIds.ToArray());
            return Collection.Find(filter).ToList(); ;
        }

        public List<Distribution> ListFromPortfolioWithProjection(int portfolioId)
        {
            var value = Collection.Find(x => x.PortfolioId == portfolioId);

            return value.ToList();
        }
    }
}
