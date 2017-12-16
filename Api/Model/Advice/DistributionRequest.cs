using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Advice
{
    public class DistributionRequest
    {
        public int AssetId { get; set; }
        public double Percent { get; set; }
    }
}
