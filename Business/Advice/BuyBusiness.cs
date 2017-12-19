using Auctus.DataAccess.Advice;
using Auctus.DomainObjects.Advice;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Advice
{
    public class BuyBusiness : BaseBusiness<Buy, BuyData>
    {
        public BuyBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Buy Create(string email, int advisorId)
        {
            var advisor = AdvisorBusiness.GetWithDetail(advisorId);
            var user = UserBusiness.GetValidUser(email);
            if (advisor.UserId == user.Id)
                throw new ArgumentException("User is the advisor owner.");

            var bought = ListBought(user.Id);
            if (bought.Any(c => c.AdvisorId == advisorId))
                throw new ArgumentException("Advisor already bought.");

            var goal = GoalBusiness.GetCurrent(user.Id);
            
            var buy = new Buy();
            buy.AdvisorId = advisorId;
            buy.CreationDate = DateTime.UtcNow;
            buy.GoalId = goal.Id;
            buy.ExpirationDate = buy.CreationDate.AddDays(advisor.Detail.Period);
            Data.Insert(buy);
            return buy;
        }

        public List<Buy> ListBought(int userId)
        {
            return Data.ListBought(userId);
        }
    }
}
