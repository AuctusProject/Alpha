using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advice
{
    public class PortfolioRequest : ProjectionRequest
    {
        public int AdvisorId { get; set; }
        public int Risk { get; set; }
    }
}
