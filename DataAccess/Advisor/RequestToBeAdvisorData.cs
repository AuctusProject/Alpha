using Auctus.DomainObjects.Advisor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class RequestToBeAdvisorData : BaseSQL<RequestToBeAdvisor>
    {
        public override string TableName => "RequestToBeAdvisor";

        private const string SELECT_BY_USER = @"SELECT r.* FROM 
                                                RequestToBeAdvisor r
                                                WHERE
                                                r.UserId = @UserId
                                                AND r.CreationDate = (SELECT MAX(r2.CreationDate) FROM [RequestToBeAdvisor] r2 WHERE r2.UserId = r.UserId) ";

        public RequestToBeAdvisor GetByUser(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return Query<RequestToBeAdvisor>(SELECT_BY_USER, parameters).SingleOrDefault();
        }
    }
}
