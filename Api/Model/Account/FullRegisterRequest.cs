using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Account
{
    public class FullRegisterRequest
    {
        public User User { get; set; }
        public Goal Goal { get; set; }
    }
}
