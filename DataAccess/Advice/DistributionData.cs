using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class DistributionData : BaseData<Distribution>
    {
        public override string TableName => "Distribution";
        
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
    }
}
