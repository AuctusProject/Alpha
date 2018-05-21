using Api.Model.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advisor
{
    public class FollowRequest
    {
        public int PortfolioId { get; set; }
        public string Address { get; set; }
    }
}
