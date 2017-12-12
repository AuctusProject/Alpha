using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class GoalOptionsData : BaseData<GoalOptions>
    {
        public override string TableName => "GoalOptions";
    }
}
