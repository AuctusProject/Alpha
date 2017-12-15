using Auctus.DomainObjects.Advice;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advice
{
    public class AdvisorData : BaseData<Advisor>
    {
        public override string TableName => "Advisor";

        private const string SELECT_WITH_OWNER = "SELECT a.* FROM Advisor a INNER JOIN [User] u ON u.Id = a.UserId WHERE u.Email = @Email AND a.Id = @Id";

        public Advisor GetWithOwner(int id, string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Email", id, DbType.AnsiString);
            return Query<Advisor>(SELECT_WITH_OWNER, parameters).SingleOrDefault();
        }
    }
}
