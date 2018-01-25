using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advisor
{
    public class BuyRequest
    {
        public int AdvisorId { get; set; }
        public int? Risk { get; set; }
    }
}
