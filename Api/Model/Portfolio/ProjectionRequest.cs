using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Portfolio
{
    public class ProjectionRequest
    {
        public int Risk { get; set; }
        public double ProjectionValue { get; set; }
        public double? OptimisticProjection { get; set; }
        public double? PessimisticProjection { get; set; }
        public List<DistributionRequest> Distribution { get; set; }
    }
}
