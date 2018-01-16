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
        public double Price { get; set; }
        public int Period { get; set; }
        public bool Purchased { get; set; }
        public int PurchaseQuantity { get; set; }
        public Dictionary<int, double> RiskProjection { get; set; }
    }
}
