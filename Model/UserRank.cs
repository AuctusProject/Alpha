using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class UserRank
    {
		public decimal InvestedAmount { get; set; }
		public decimal ReturnPercentage { get; set; }
		public int Rank { get; set; }
		public decimal TotalAmount { get; set; }
		public string Username { get; set; }
    }
}
