using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class AdvisorDetailData : BaseData<AdvisorDetail>
    {
        public override string TableName => "AdvisorDetail";

        private const string SELECT_AUTO_ENABLED = @"SELECT d.* FROM 
                                                     AdvisorDetail d 
                                                     WHERE 
                                                     d.AdvisorId = @AdvisorId AND 
                                                     d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = d.AdvisorId) AND
                                                     d.Enabled = 0 AND
                                                     NOT EXISTS (SELECT 1 FROM Portfolio p WHERE p.AdvisorId = d.AdvisorId)";

        public AdvisorDetail GetForAutoEnabled(int advisorId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("AdvisorId", advisorId, DbType.Int32);
            return Query<AdvisorDetail>(SELECT_AUTO_ENABLED, parameters).SingleOrDefault();
        }
    }
}
