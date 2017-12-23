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

        private const string SELECT_WITH_DETAIL = @"SELECT a.*, d.* FROM
                                                    Advisor a
                                                    INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                    WHERE
                                                    a.Id = @Id AND d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) ";

        private const string LIST_ALL_AVAILABLE_WITH_DETAIL = @"SELECT a.*, d.* FROM
                                                    Advisor a
                                                    INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                    WHERE
                                                    d.Enabled = 1 AND d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) ";

        public Advisor GetWithOwner(int id, string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Email", id, DbType.AnsiString);
            return Query<Advisor>(SELECT_WITH_OWNER, parameters).SingleOrDefault();
        }

        public Advisor GetWithDetail(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            return Query<Advisor, AdvisorDetail, Advisor>(SELECT_WITH_DETAIL,
                    (ad, de) =>
                    {
                        ad.Detail = de;
                        return ad;
                    }, "Id", parameters).SingleOrDefault();
        }

        public IEnumerable<Advisor> ListAvailable()
        {
            return Query<Advisor, AdvisorDetail, Advisor>(LIST_ALL_AVAILABLE_WITH_DETAIL,
                    (ad, de) =>
                    {
                        ad.Detail = de;
                        return ad;
                    }, "Id");
        }
    }
}
