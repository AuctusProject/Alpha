using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class ProjectionData : BaseData<Projection>
    {
        public override string TableName => "Projection";

        public Projection Get(int projectionId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", projectionId, DbType.Int32);
            return SelectByParameters<Projection>(parameters).SingleOrDefault();
        }
    }
}
