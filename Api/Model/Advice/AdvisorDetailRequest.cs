using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advice
{
    public class AdvisorDetailRequest
    {
        public int IdAdvisor { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public int Period { get; set; }
        public bool Enabled { get; set; }
    }
}
