using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class GoalData : BaseData<Goal>
    {
        public override string TableName => "Goal";
    }
}
