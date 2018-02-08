using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Portfolio
{
    public class PortfolioRequest : ProjectionRequest
    {
        public int AdvisorId { get; set; }
        public double Price { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
