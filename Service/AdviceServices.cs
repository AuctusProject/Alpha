using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Service
{
    public class AdviceServices : BaseServices
    {
        public AdviceServices(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }
    }
}
