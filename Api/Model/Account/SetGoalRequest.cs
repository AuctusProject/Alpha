﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Model.Account
{
    public class SetGoalRequest
    {
        public double? TargetAmount { get; set; }
        public double StartingAmount { get; set; }
        public double MonthlyContribution { get; set; }
        public int Timeframe { get; set; }
        public int Risk { get; set; }
        public int GoalOptionId { get; set; }
    }
}
