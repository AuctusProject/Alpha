using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class GoalOptionData : BaseData<GoalOption>
    {
        public override string TableName => "GoalOption";
    }
}
