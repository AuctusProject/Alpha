using Auctus.Util.DapperAttributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Advisor
{
    public class RequestToBeAdvisor
    {
        [DapperKey(true)]
        [DapperType(System.Data.DbType.Int32)]
        public int Id { get; set; }
        [DapperType(System.Data.DbType.Int32)]
        public int UserId { get; set; }
        [DapperType(System.Data.DbType.DateTime)]
        public DateTime CreationDate { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Name { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string Description { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string PreviousExperience { get; set; }
        [DapperType(System.Data.DbType.AnsiString)]
        public string OtherService { get; set; }
        [DapperType(System.Data.DbType.Boolean)]
        public bool RecommendPortfolios { get; set; }
        [DapperType(System.Data.DbType.Boolean)]
        public bool ResearchReports { get; set; }
        [DapperType(System.Data.DbType.Boolean)]
        public bool PersonalizedAdvice { get; set; }
        [DapperType(System.Data.DbType.Boolean)]
        public bool? Approved { get; set; }
    }
}
