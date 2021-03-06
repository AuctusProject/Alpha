﻿using Auctus.DataAccess.Account;
using Auctus.DomainObjects.Account;
using Auctus.Util;
using Microsoft.AspNetCore.NodeServices;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Auctus.Business.Account
{
    public class GoalBusiness : BaseBusiness<Goal, GoalData>
    {
        public GoalBusiness(ILoggerFactory loggerFactory, Cache cache, INodeServices nodeServices) : base(loggerFactory, cache, nodeServices) { }

        public Goal Create(string email, int goalOptionId, int timeframe, int risk, double? targetAmount, double startingAmount, double monthlyContribution)
        {
            var user = UserBusiness.GetValidUser(email);
            return Create(user.Id, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
        }

        public Goal Create(int userId, int goalOptionId, int timeframe, int risk, double? targetAmount, double startingAmount, double monthlyContribution)
        {
            var goal = SetNew(userId, goalOptionId, timeframe, risk, targetAmount, startingAmount, monthlyContribution);
            Data.Insert(goal);
            return goal;
        }
        
        public Goal GetCurrent(int userId)
        {
            var goal = Data.GetCurrent(userId);
            if (goal == null)
                throw new ArgumentException("An user goal must be defined.");

            var options = GoalOptionsBusiness.List();
            goal.GoalOption = options.Single(c => c.Id == goal.GoalOptionId);
            return goal;
        }

        public Goal SetNew(int userId, int goalOptionId, int timeframe, int risk, double? targetAmount, double startingAmount, double monthlyContribution)
        {
            GoalOptionsBusiness.Get(goalOptionId);
            RiskType riskType = RiskType.Get(risk);
            if (timeframe <= 0)
                throw new ArgumentException("Invalid timeframe for goal.");
            if ((startingAmount == 0 && monthlyContribution == 0) || startingAmount < 0 || monthlyContribution < 0)
                throw new ArgumentException("Invalid contribution.");

            var goal = new Goal();
            goal.UserId = userId;
            goal.GoalOptionId = goalOptionId;
            goal.CreationDate = DateTime.UtcNow;
            goal.MonthlyContribution = monthlyContribution;
            goal.StartingAmount = startingAmount;
            goal.TargetAmount = targetAmount;
            goal.Timeframe = timeframe;
            goal.Risk = riskType.Value;
            return goal;
        }
    }
}
