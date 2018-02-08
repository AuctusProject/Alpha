using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DomainObjects.Account
{
    public enum TransactionStatus : int
    {
        Pending = 0,
        Success = 1,
        Error = 2,
        Cancel = 3,
        Lost = 4
    }
}
