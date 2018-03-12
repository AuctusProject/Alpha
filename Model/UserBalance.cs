using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class UserBalance
    {
        public int UserId { get; set; }
		public string Username { get; set; }
		public decimal TotalAmount { get; set; }
        public decimal InvestedAmount { get; set; }
        public decimal AvailableAmount { get; set; }
    }
}
