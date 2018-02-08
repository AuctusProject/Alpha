using Auctus.DataAccess.Advisor;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Advisor
{
    public class AdvisorDetailBusiness : BaseBusiness<AdvisorDetail, AdvisorDetailData>
    {
        public AdvisorDetailBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public AdvisorDetail SetNew(int advisorId, string name, string description, bool enabled)
        {
            ValidateBaseCreation(name, description);
            var advisorDetail = new AdvisorDetail();
            advisorDetail.AdvisorId = advisorId;
            advisorDetail.Date = DateTime.UtcNow;
            advisorDetail.Description = description;
            advisorDetail.Name = name;
            advisorDetail.Enabled = enabled; 
            return advisorDetail;
        }

        public AdvisorDetail Create(string email, int advisorId, string name, string description)
        {
            var advisor = AdvisorBusiness.GetWithOwner(advisorId, email);
            if (advisor == null)
                throw new ArgumentException("Invalid advisor.");

            var advisorDetail = SetNew(advisor.Id, name, description, true);
            Data.Insert(advisorDetail);
            return advisorDetail;
        }

        public AdvisorDetail Disable(string email, int advisorId)
        {
            var advisor = AdvisorBusiness.GetWithOwner(advisorId, email);
            if (advisor == null)
                throw new ArgumentException("Invalid advisor.");

            var oldDetail = GetByAdvisor(advisor.Id);
            var advisorDetail = SetNew(advisor.Id, oldDetail.Name, oldDetail.Description, false);
            Data.Insert(advisorDetail);
            return advisorDetail;
        }

        private AdvisorDetail GetByAdvisor(int advisorId)
        {
            return Data.GetByAdvisor(advisorId);
        }

        private void ValidateBaseCreation(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name cannot be empty.");
            if (name.Length > 100)
                throw new ArgumentException("Name is too long.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description cannot be empty.");
            if (description.Length > 500)
                throw new ArgumentException("Description is too long.");
        }
        
        public AdvisorDetail GetForAutoEnabled(int advisorId)
        {
            return Data.GetForAutoEnabled(advisorId);
        }
    }
}
