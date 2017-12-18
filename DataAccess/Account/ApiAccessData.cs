using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class ApiAccessData : BaseData<ApiAccess>
    {
        public override string TableName => "ApiAccess";
    }
}
