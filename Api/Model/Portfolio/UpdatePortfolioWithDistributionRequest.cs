using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Portfolio
{
    public class UpdatePortfolioWithDistributionRequest : UpdatePortfolioRequest
    {
        public List<DistributionRequest> Distribution { get; set; }
    }
}
