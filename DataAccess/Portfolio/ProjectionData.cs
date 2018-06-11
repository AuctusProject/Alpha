using Auctus.DomainObjects.Portfolio;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Portfolio
{
    public class ProjectionData : BaseSQL<Projection>
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
