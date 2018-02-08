using Api.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advisor
{
    public class BuyRequest
    {
        public int PortfolioId { get; set; }
        public int Days { get; set; }
        public string Address { get; set; }
        public SetGoalRequest Goal { get; set; }
    }
}
