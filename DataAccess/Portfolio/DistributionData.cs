using Auctus.DomainObjects.Portfolio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Portfolio
{
    public class DistributionData : BaseSQL<Distribution>
    {
        public override string TableName => "Distribution";

        private const string LIST_DISTRIBUTIONS_FROM_PORTFOLIO = 
            @"SELECT d.*, p.* FROM Distribution d 
                INNER JOIN Projection p ON p.Id = d.ProjectionId
                WHERE p.PortfolioId = @PortfolioId";
        
        public List<Distribution> List(IEnumerable<int> projectionsId)
        {
            List<string> restrictions = new List<string>();
            DynamicParameters parameters = new DynamicParameters();
            for (int i = 0; i < projectionsId.Count(); ++i)
            {
                var key = string.Format("ProjectionId{0}", i);
                restrictions.Add(string.Format("ProjectionId = @{0}", key));
                parameters.Add(key, projectionsId.ElementAt(i), DbType.Int32);
            }
            return Query<Distribution>(string.Format("SELECT * FROM Distribution WHERE {0}", string.Join(" OR ", restrictions)), parameters).ToList();
        }

        public List<Distribution> ListFromPortfolioWithProjection(int portfolioId)
        {
            List<string> restrictions = new List<string>();
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("PortfolioId", portfolioId, DbType.Int32);            
            return Query<Distribution, Projection, Distribution>(LIST_DISTRIBUTIONS_FROM_PORTFOLIO,
                           (distribution, proj) =>
                           {
                               distribution.Projection = proj;
                               return distribution;
                           }, "Id", parameters).ToList();
        }
    }
}
