﻿using Auctus.DomainObjects.Account;
using System;
using System.Collections.Generic;
using System.Text;

namespace Auctus.DataAccess.Account
{
    public class TransactionData : BaseSQL<Transaction>
    {
        public override string TableName => "Transaction";
    }
}
