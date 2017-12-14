using Auctus.DataAccess.Advice;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Advice
{
    public class AdvisorDetailBusiness : BaseBusiness<AdvisorDetail, AdvisorDetailData>
    {
        public AdvisorDetailBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }
    }
}
