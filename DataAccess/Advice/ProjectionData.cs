using Auctus.DomainObjects.Advice;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class ProjectionData : BaseData<Projection>
    {
        public override string TableName => "Projection";
    }
}
