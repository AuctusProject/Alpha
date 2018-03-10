using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class UserBalance
    {
        public int UserId { get; set; }
        public decimal InvestedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
    }
}
