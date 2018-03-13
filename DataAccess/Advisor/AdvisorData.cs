﻿using Auctus.DomainObjects.Advisor;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace Auctus.DataAccess.Advisor
{
    public class AdvisorData : BaseData<DomainObjects.Advisor.Advisor>
    {
        public override string TableName => "Advisor";

        private const string SELECT_WITH_OWNER = "SELECT a.* FROM Advisor a INNER JOIN [User] u ON u.Id = a.UserId WHERE u.Email = @Email AND a.Id = @Id";

        private const string SELECT_WITH_DETAIL = @"SELECT a.*, d.* FROM
                                                    Advisor a
                                                    INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                    WHERE
                                                    a.Id = @Id AND d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) ";

        private const string LIST_ALL_ROBO_AVAILABLE_WITH_DETAIL = @"SELECT a.*, d.* FROM
                                                                     Advisor a
                                                                     INNER JOIN AdvisorDetail d ON d.AdvisorId = a.Id
                                                                     WHERE a.Type = 1 AND
                                                                     d.Enabled = 1 AND d.Date = (SELECT max(d2.Date) FROM AdvisorDetail d2 WHERE d2.AdvisorId = a.Id) ";

        private const string LIST_ADVISORS_RANK_BY_AUC = @"SELECT 
                                                                adv.Id, advd.Id, advd.Name,  sum(buy.Price) as  [AllocatedAUC] 
                                                            FROM 
                                                                advisor adv INNER JOIN 
	                                                            advisorDetail advd on advd.advisorId = adv.id INNER JOIN
	                                                            portfolio ptf on ptf.AdvisorId = adv.Id INNER JOIN
	                                                            portfolioDetail ptfd on ptfd.PortfolioId = ptf.id INNER JOIN
	                                                            buy buy on buy.portfolioId = ptf.Id
                                                            WHERE 
                                                                advd.enabled = 1 and ptfd.enabled = 1
                                                            GROUP BY 
                                                                adv.id, advd.Id, advd.Name
                                                            ORDER BY
                                                                [AllocatedAUC] desc";

        public DomainObjects.Advisor.Advisor GetWithOwner(int id, string email)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            parameters.Add("Email", email, DbType.AnsiString);
            return Query<DomainObjects.Advisor.Advisor>(SELECT_WITH_OWNER, parameters).SingleOrDefault();
        }

        public DomainObjects.Advisor.Advisor SimpleGetByOwner(int userId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("UserId", userId, DbType.Int32);
            return SelectByParameters<DomainObjects.Advisor.Advisor>(parameters).SingleOrDefault();
        }

        public DomainObjects.Advisor.Advisor GetWithDetail(int id)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("Id", id, DbType.Int32);
            return Query<DomainObjects.Advisor.Advisor, AdvisorDetail, DomainObjects.Advisor.Advisor>(SELECT_WITH_DETAIL,
                    (ad, de) =>
                    {
                        ad.Detail = de;
                        return ad;
                    }, "Id", parameters).SingleOrDefault();
        }

        public IEnumerable<DomainObjects.Advisor.Advisor> ListRobosAvailable()
        {
            return Query<DomainObjects.Advisor.Advisor, AdvisorDetail, DomainObjects.Advisor.Advisor>(LIST_ALL_ROBO_AVAILABLE_WITH_DETAIL,
                    (ad, de) =>
                    {
                        ad.Detail = de;
                        return ad;
                    }, "Id");
        }

        public IEnumerable<DomainObjects.Advisor.Advisor> ListAdvisorsRankByAUC()
        {
            return Query<DomainObjects.Advisor.Advisor, AdvisorDetail, DomainObjects.Advisor.Advisor>(LIST_ADVISORS_RANK_BY_AUC,
                    (ad, de) =>
                    {
                        ad.Detail = de;
                        return ad;
                    }, "Id");
        }
    }
}
