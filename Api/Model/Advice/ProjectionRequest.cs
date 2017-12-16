using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advice
{
    public class ProjectionRequest
    {
        public int? PortfolioId { get; set; }
        public double ProjectionValue { get; set; }
        public double? OptimisticProjection { get; set; }
        public double? PessimisticProjection { get; set; }
        public List<DistributionRequest> Distribution { get; set; }
    }
}
