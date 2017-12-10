using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class UserData : BaseData<User>
    {
        public override string TableName => "[User]";
    }
}
