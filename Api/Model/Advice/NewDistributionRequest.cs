using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advice
{
    public class NewDistributionRequest
    {
        public int PortfolioId { get; set; }
        public List<DistributionRequest> Distribution { get; set; }
    }
}
