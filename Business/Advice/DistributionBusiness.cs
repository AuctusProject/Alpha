using Auctus.DataAccess.Advice;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Advice
{
    public class DistributionBusiness : BaseBusiness<Distribution, DistributionData>
    {
        public DistributionBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public List<Distribution> SetNew(int projectionId, Dictionary<int, double> distribution)
        {
            if (distribution.Any(c => c.Value < 0 || c.Value > 100))
                throw new ArgumentException("Invalid asset distribution value.");
            if (distribution.Sum(c => c.Value) != 100)
                throw new ArgumentException("Asset distribution must match 100%.");

            var distributions = new List<Distribution>();
            foreach(KeyValuePair<int, double> key in distribution)
            {
                if (key.Value > 0)
                    distributions.Add(new Distribution()
                    {
                        AssetId = key.Key,
                        Percent = key.Value,
                        ProjectionId = projectionId
                    });
            }
            return distributions;
        }

        public List<Distribution> List(IEnumerable<int> projectionIds)
        {
            if (projectionIds.Count() == 0)
                return new List<Distribution>();

            var distributions = Data.List(projectionIds);
            var assets = AssetBusiness.ListAssets();
            distributions.ForEach(c => c.Asset = assets.Single(a => a.Id == c.AssetId));
            return distributions;
        }
    }
}
