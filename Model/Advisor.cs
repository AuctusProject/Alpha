using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class Advisor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Owned { get; set; }
        public bool Enabled { get; set; }
        public int PurchaseQuantity { get; set; }
        public List<Portfolio> Portfolio { get; set; }
    }
}
