using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Model
{
    public class CheckTransaction
    {
        public int Status { get; set; }
        public List<Portfolio.Distribution> Distribution { get; set; }
}
}
