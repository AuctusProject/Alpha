using Auctus.DataAccess.Advisor;
using Auctus.DomainObjects.Account;
using Auctus.DomainObjects.Advisor;
using Auctus.Util;
using Auctus.Util.NotShared;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Auctus.Business.Advisor
{
    public class RequestToBeAdvisorBusiness : BaseBusiness<RequestToBeAdvisor, RequestToBeAdvisorData>
    {
        public RequestToBeAdvisorBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public async Task<RequestToBeAdvisor> Create(string email, string name, string description, string previousExperience, 
            bool recommendPortfolios, bool researchReports, bool personalizedAdvice, string otherServiceProvided)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Name must be filled.");
            if (name.Length > 100)
                throw new ArgumentException("Name cannot have more than 100 characters.");
            if (string.IsNullOrWhiteSpace(description))
                throw new ArgumentException("Description must be filled.");
            if (description.Length > 4000)
                throw new ArgumentException("Description cannot have more than 4000 characters.");
            if (string.IsNullOrWhiteSpace(previousExperience))
                throw new ArgumentException("Previous experience must be filled.");
            if (previousExperience.Length > 4000)
                throw new ArgumentException("Previous experience cannot have more than 4000 characters.");
            if (!recommendPortfolios && !researchReports && !personalizedAdvice && string.IsNullOrWhiteSpace(otherServiceProvided))
                throw new ArgumentException("Service provided must be selected.");
            if (otherServiceProvided != null && otherServiceProvided.Length > 150)
                throw new ArgumentException("Other service provided cannot have more than 150 characters.");

            var user = UserBusiness.GetValidUser(email);
            if (user?.ConfirmationDate == null)
                throw new ArgumentException("User didn't confirm e-mail.");

            var request = GetByUser(user.Id);
            if (request?.Approved ?? false)
                throw new ArgumentException("User was already approved as advisor.");

            var newRequest = new RequestToBeAdvisor()
            {
                CreationDate = DateTime.UtcNow,
                Name = name,
                Description = description,
                PreviousExperience = previousExperience,
                UserId = user.Id,
                RecommendPortfolios = recommendPortfolios,
                ResearchReports = researchReports,
                PersonalizedAdvice = personalizedAdvice,
                OtherService = otherServiceProvided
            };
            Data.Insert(newRequest);
            await SendRequestToBeAdvisorEmail(user, newRequest, request);
            return newRequest;
        }
        private async Task SendRequestToBeAdvisorEmail(User user, RequestToBeAdvisor newRequestToBeAdvisor, RequestToBeAdvisor oldRequestToBeAdvisor)
        {
            await Email.SendAsync(Config.EMAIL_FOR_CRITICAL_ERROR,
                string.Format("[{0}] Request to be adivosr - Auctus Beta", oldRequestToBeAdvisor == null ?  "NEW" : "UPDATE"),
                string.Format(@"Email: {0} // Username: {1}
<br/>
<br/>
<b>Old Name</b>: {2}
<br/>
<b>New Name</b>: {3}
<br/>
<br/>
<b>Old Description</b>: {4}
<br/>
<b>New Description</b>: {5}
<br/>
<br/>
<b>Old Previous Experience</b>: {6}
<br/>
<b>New Previous Experience</b>: {7}
<br/>
<br/>
<b>New Portfolio Recommendations</b>: {8} // <b>Old Portfolio Recommendations</b>: {9}
<br/>
<br/>
<b>New Research Reports</b>: {10} // <b>Old Research Reports</b>: {11}
<br/>
<br/>
<b>New Personalized Advice</b>: {12} // <b>Old Personalized Advice</b>: {13}
<br/>
<br/>
<b>New Other Service</b>: {14} // <b>Old Other Service</b>: {15}", user.Email, user.Username, 
oldRequestToBeAdvisor?.Name ?? "N/A", newRequestToBeAdvisor.Name,
oldRequestToBeAdvisor?.Description ?? "N/A", newRequestToBeAdvisor.Description,
oldRequestToBeAdvisor?.PreviousExperience ?? "N/A", newRequestToBeAdvisor.PreviousExperience,
newRequestToBeAdvisor.RecommendPortfolios, oldRequestToBeAdvisor == null ? "N/A" : oldRequestToBeAdvisor.RecommendPortfolios.ToString(),
newRequestToBeAdvisor.ResearchReports, oldRequestToBeAdvisor == null ? "N/A" : oldRequestToBeAdvisor.ResearchReports.ToString(),
newRequestToBeAdvisor.PersonalizedAdvice, oldRequestToBeAdvisor == null ? "N/A" : oldRequestToBeAdvisor.PersonalizedAdvice.ToString(),
newRequestToBeAdvisor.OtherService, oldRequestToBeAdvisor?.OtherService ?? "N/A"));
        }

        public RequestToBeAdvisor GetByUser(string email)
        {
            var user = UserBusiness.GetValidUser(email);
            return GetByUser(user.Id);
        }

        public RequestToBeAdvisor GetByUser(int userId)
        {
            return Data.GetByUser(userId);
        }
    }
}
