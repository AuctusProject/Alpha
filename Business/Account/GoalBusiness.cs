using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.Business.Account
{
    public class GoalBusiness : BaseBusiness<Goal, GoalData>
    {
        public GoalBusiness(ILoggerFactory loggerFactory, Cache cache) : base(loggerFactory, cache) { }

        public Goal Create(string email, int goalOptionId, int? timeframe, int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            var user = UserBusiness.GetValidUser(email);
            return Create(user.Id, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public Goal Create(int userId, int goalOptionId, int? timeframe, int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            var goal = SetNewData(userId, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
            Data.Insert(goal);
            return goal;
        }

        public Goal SetNewData(int userId, int goalOptionId, int? timeframe, int? risk, double? targetAmount, double? startingAmount, double? monthlyContribution)
        {
            var goal = new Goal();
            goal.UserId = userId;
            goal.GoalOptionId = goalOptionId;
            goal.CreationDate = DateTime.UtcNow;
            goal.MonthlyContribution = monthlyContribution;
            goal.StartingAmount = startingAmount;
            goal.TargetAmount = targetAmount;
            goal.Timeframe = timeframe;
            goal.Risk = risk;
            return goal;
        }
    }
}
